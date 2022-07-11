using Furion;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAParser;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="IAuthService"/>
    [Route("Auth"), ApiDescriptionSettings(Name = "Auth", Order = 999)]
    public class AuthService : IAuthService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysUser> _sysUserRep;
        private readonly SqlSugarRepository<SysLogVis> _sysLogVisRep;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISysEmpService _sysEmpService;
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysMenuService _sysMenuService;
        private readonly ISysConfigService _sysConfigService;
        private readonly IEventPublisher _eventPublisher;

        public AuthService(SqlSugarRepository<SysUser> sysUserRep, SqlSugarRepository<SysLogVis> sysLogVisRep,
            IHttpContextAccessor httpContextAccessor,
            ISysEmpService sysEmpService, ISysRoleService sysRoleService, ISysMenuService sysMenuService,
            ISysConfigService sysConfigService, IEventPublisher eventPublisher
            )
        {
            _sysUserRep = sysUserRep;
            _sysLogVisRep = sysLogVisRep;
            _httpContextAccessor = httpContextAccessor;
            _sysEmpService = sysEmpService;
            _sysRoleService = sysRoleService;
            _sysMenuService = sysMenuService;
            _sysConfigService = sysConfigService;
            _eventPublisher = eventPublisher;
        }

        /// <inheritdoc/>
        [HttpGet("getUserinfo")]
        public async Task<LoginInfoResult> GetUserinfo()
        {

            var user = _sysUserRep.Single(AppUser.UserId);
            var userId = user.Id;

            var httpContext = App.GetService<IHttpContextAccessor>().HttpContext;
            var loginOutput = user.Adapt<LoginInfoResult>();

            loginOutput.LastLoginTime = user.LastLoginTime = DateTime.Now;
            var ip = HttpNewUtil.Ip;
            loginOutput.LastLoginIp = user.LastLoginIp =
                string.IsNullOrEmpty(user.LastLoginIp) ? HttpNewUtil.Ip : ip;

            var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            loginOutput.LastLoginBrowser = clent.UA.Family + clent.UA.Major;
            loginOutput.LastLoginOs = clent.OS.Family + clent.OS.Major;

            // 员工信息
            loginOutput.LoginEmpInfo = await _sysEmpService.GetEmpInfo(userId);

            // 角色信息
            loginOutput.Roles = await _sysRoleService.GetUserRoleList(userId);

            //// 权限信息
            //loginOutput.Permissions = await _sysMenuService.GetLoginPermissionList(userId);

            // 数据范围信息(机构Id集合)
            loginOutput.DataScopes = await DataFilterExtensions.GetDataScopeIdList();

            // 增加登录日志
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = user.RealName,
                    Success = YesOrNo.Yes,
                    Message = "登录成功",
                    Location = httpContext?.Request?.GetRequestUrlAddress(),
                    Ip = loginOutput.LastLoginIp,
                    Browser = loginOutput.LastLoginBrowser,
                    Os = loginOutput.LastLoginOs,
                    VisType = LoginType.LOGIN,
                    VisTime = loginOutput.LastLoginTime,
                    Account = user.Account
                }));

            return loginOutput;
        }

        /// <inheritdoc/>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<LoginResult> Login([Required] LoginParam param)
        {
            // 获取加密后的密码
            var encryptPassword = MD5Encryption.Encrypt(param.Password);

            // 判断用户名和密码是否正确(排除全局多租户过滤器)Filter(null,true)
            var user = _sysUserRep.AsQueryable().Filter(null, true)
                .First(u =>
                    u.Account.Equals(param.Account) && u.Password.Equals(encryptPassword) && !u.IsDeleted);
            _ = user ?? throw Oops.Oh(ErrorCode.E1000);

            // 验证账号是否被冻结
            if (user.Status == ValidityStatus.DISABLE)
                throw Oops.Oh(ErrorCode.E1017);

            // 生成Token令牌            
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                {ConstClaim.UserId, user.Id},
                {ConstClaim.Account, user.Account},
                {ConstClaim.Name, user.RealName},
                {ConstClaim.SuperAdmin, user.UserType},
            });

            // 设置Swagger自动登录
            _httpContextAccessor.HttpContext.SigninToSwagger(accessToken);

            // 生成刷新Token令牌
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 30);

            // 设置刷新Token令牌
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            var httpContext = App.HttpContext;
            await _eventPublisher.PublishAsync(new ChannelEventSource("Update:UserLoginInfo",
                new SysUser { 
                    Id = user.Id, 
                    LastLoginIp = httpContext.GetLocalIpAddressToIPv4(), 
                    LastLoginTime = DateTime.Now
                }));
            return new LoginResult { UserId = user.Id, Token = accessToken };
        }

        /// <inheritdoc/>
        [HttpGet("logout")]
        [AllowAnonymous]
        public async Task Logout()
        {
            _httpContextAccessor.HttpContext.SignoutToSwagger();
            var ip = HttpNewUtil.Ip;
            var user = _sysUserRep.Single(AppUser.UserId);
            var clent = Parser.GetDefault().Parse(_httpContextAccessor.HttpContext.Request.Headers["User-Agent"]);
            var browser = clent.UA.Family + clent.UA.Major;
            var os = clent.OS.Family + clent.OS.Major;
            // 增加退出日志
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = user.RealName,
                    Success = YesOrNo.Yes,
                    Location = _httpContextAccessor?.HttpContext?.Request?.GetRequestUrlAddress(),
                    Message = "退出成功",
                    VisType = LoginType.LOGOUT,
                    VisTime = DateTime.Now,
                    Account = user.Account,
                    Ip = ip,
                    Browser = browser,
                    Os = os,
                }));

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        [HttpGet("getPermCode")]
        public async Task<dynamic> GetPermCode()
        {
            var user = _sysUserRep.Single(AppUser.UserId);
            var userId = user.Id;

            var httpContext = App.GetService<IHttpContextAccessor>().HttpContext;

            var ip = HttpNewUtil.Ip;
            var LastLoginIp = user.LastLoginIp =
                string.IsNullOrEmpty(user.LastLoginIp) ? HttpNewUtil.Ip : ip;

            var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            var LastLoginBrowser = clent.UA.Family + clent.UA.Major;
            var LastLoginOs = clent.OS.Family + clent.OS.Major;

            // 权限信息
            var Permissions = await _sysMenuService.GetLoginPermissionList(userId);

            // 增加登录日志
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = user.RealName,
                    Success = YesOrNo.Yes,
                    Message = "获取权限信息",
                    Ip = LastLoginIp,
                    Browser = LastLoginBrowser,
                    Os = LastLoginOs,
                    Location = httpContext?.Request?.GetRequestUrlAddress(),
                    VisType = LoginType.PERMISSIONS,
                    VisTime = DateTime.Now,
                    Account = user.Account
                }));

            return Permissions;
        }

    }
}
