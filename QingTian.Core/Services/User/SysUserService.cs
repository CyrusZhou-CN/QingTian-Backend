
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingTian.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services.User
{
    /// <inheritdoc cref="ISysUserService" />
    [Route("SysUser"), ApiDescriptionSettings(Name = "User", Order = 0)]
    public class SysUserService : ISysUserService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysUser> _sysUserRep;  // 用户表

        private readonly ISysCacheService _sysCacheService;
        private readonly ISysConfigService _sysConfigService;
        private readonly ISysEmpService _sysEmpService;
        private readonly ISysUserDataScopeService _sysUserDataScopeService;
        private readonly ISysUserRoleService _sysUserRoleService;
        public SysUserService(SqlSugarRepository<SysUser> sysUserRep,
                              ISysCacheService sysCacheService,
                              ISysEmpService sysEmpService,
                              ISysUserDataScopeService sysUserDataScopeService,
                              ISysUserRoleService sysUserRoleService,
                              ISysConfigService sysConfigService)
        {
            _sysUserRep = sysUserRep;
            _sysCacheService = sysCacheService;
            _sysEmpService = sysEmpService;
            _sysUserDataScopeService = sysUserDataScopeService;
            _sysUserRoleService = sysUserRoleService;
            _sysConfigService = sysConfigService;
        }

        #region HttpGet


        /// <inheritdoc/>
        [HttpGet("isAccountExist")]
        public async Task IsAccountExist([FromQuery] QueryUserExistParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Account == param.Account && u.Id != param.Id);
            if (!user.IsNullOrZero())
            {
                throw Oops.Oh(ErrorCode.E1003);
            }
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<dynamic> GetUser([FromQuery] QueryUserParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            var  userView = user.Adapt<UserView>();
            if (!userView.IsNullOrZero()) {
                userView.SysEmpParam = await _sysEmpService.GetEmpInfo(user.Id);
            }

            return userView;
        }

        /// <inheritdoc/>
        [HttpGet("ownData")]
        public async Task<dynamic> GetUserOwnData([FromQuery] QueryUserParam param)
        {
            return await _sysUserDataScopeService.GetUserDataScopeIdListAsync(param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("ownRole")]
        public async Task<dynamic> GetUserOwnRole([FromQuery] QueryUserParam param)
        {
            return await _sysUserRoleService.GetUserRoleIdListAsync(param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("selector")]
        public async Task<dynamic> GetUserSelector([FromQuery] UserParam param)
        {
            return await _sysUserRep.AsQueryable()
                                  .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => (u.RealName.Contains(param.Name.Trim())))
                                  .Where(u => u.Status != ValidityStatus.DELETED)
                                  //.Where(u => u.UserType != UserType.SuperAdmin)
                                  .Select(u => new
                                  {
                                      u.Id,
                                      u.RealName
                                  }).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryUserPageList([FromQuery] UserParam param)
        {
            var superAdmin = AppUser.IsSuperAdmin;
            var searchValue = param.SearchValue;
            var pid = param.SysEmpParam.OrgId;
            var users = await _sysUserRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(searchValue), u => u.Account.Contains(param.SearchValue.Trim()) ||
                                                                            u.RealName.Contains(param.SearchValue.Trim()) ||
                                                                            u.Phone.Contains(param.SearchValue.Trim()))
                .WhereIF(Enum.IsDefined(typeof(ValidityStatus), param.SearchStatus), u => u.Status == param.SearchStatus)
                .WhereIF(!superAdmin, u => u.UserType == UserType.User)
               .ToPagedListAsync(param.Page, param.PageSize);
            return users.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpGet("export")]
        public async Task<IActionResult> ExportUser([FromQuery] UserParam param)
        {
            var users = await _sysUserRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(param.Account), u => u.Account.Contains(param.Account.Trim()))
                .WhereIF(!string.IsNullOrWhiteSpace(param.NickName), u => u.NickName.Contains(param.NickName.Trim()))
                .ToListAsync();
            // MiniExcel enum 类型的值不能为空
            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(users);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return await Task.FromResult(new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "user.xlsx"
            });
        }
        #endregion

        #region HttpPost

        /// <inheritdoc/>
        [HttpPost("import")]
        public async Task ImportUser(IFormFile file)
        {
            var path = Path.Combine(Path.GetTempPath(), $"{SnowFlakeSingle.Instance.NextId()}.xlsx");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            var rows = MiniExcel.Query(path); // 解析
            foreach (var row in rows)
            {
                //TODO:导入用户操作
                var a = row.A;
                var b = row.B;
                // 导入操作
            }
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddUser(AddUserParam param)
        {
            // 数据范围检查
            CheckDataScope(param.SysEmpParam == null || string.IsNullOrEmpty(param.SysEmpParam.OrgId) ? 0 : long.Parse(param.SysEmpParam.OrgId));

            var isExist = await _sysUserRep.AnyAsync(u => u.Account == param.Account);
            if (isExist) throw Oops.Oh(ErrorCode.E1003);

            var user = param.Adapt<SysUser>();
            user.UserType = UserType.User;
            user.Password = MD5Encryption.Encrypt(param.Password);
            if (string.IsNullOrEmpty(user.RealName))
                user.RealName = user.Account;
            if (string.IsNullOrEmpty(user.NickName))
                user.NickName = user.Account;

            try
            {
                _sysUserRep.Ado.BeginTran();
                var newUser = await _sysUserRep.InsertReturnEntityAsync(user);
                param.SysEmpParam.Id = newUser.Id;
                // 增加员工信息
                await _sysEmpService.AddOrUpdate(param.SysEmpParam);
                _sysUserRep.Ado.CommitTran();
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            }
            catch (Exception ex)
            {
                _sysUserRep.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteUser(DeleteUserParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (user.UserType == UserType.SuperAdmin)
                throw Oops.Oh(ErrorCode.E1014);

            if (user.Account == AppUser.Account)
            {
                throw Oops.Oh(ErrorCode.E1001);
            }

            // 数据范围检查
            CheckDataScopeByUserId(param.Id);

            try
            {
                _sysUserRep.Ado.BeginTran();
                // 直接删除用户
                await _sysUserRep.AsUpdateable(new SysUser { IsDeleted = true }).CallEntityMethod(m => m.Modify())
                    .UpdateColumns(user.FalseDeleteColumn()).Where(wh => wh.Id == user.Id).ExecuteCommandAsync();

                // 删除员工及附属机构职位信息
                await _sysEmpService.DeleteEmpInfoByUserId(user.Id);

                //删除该用户对应的用户-角色表关联信息
                await _sysUserRoleService.DeleteUserRoleListByUserIdAsync(user.Id);

                //删除该用户对应的用户-数据范围表关联信息
                await _sysUserDataScopeService.DeleteUserDataScopeListByUserIdAsync(user.Id);
                _sysUserRep.Ado.CommitTran();
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            }
            catch (Exception)
            {
                _sysUserRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        [HttpPost("grantData")]
        public async Task GrantUserData(UpdateUserParam param)
        {
            // 清除缓存
            await _sysCacheService.DelAsync(ConstCache.CACHE_KEY_DATASCOPE + $"{param.Id}");
            await _sysCacheService.DelAsync(ConstCache.CACHE_KEY_USERSDATASCOPE + $"{param.Id}");
            // 数据范围检查
            CheckDataScopeByUserId(param.Id);
            await _sysUserDataScopeService.GrantDataAsync(param);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);
        }

        /// <inheritdoc/>
        [HttpPost("grantRole")]
        public async Task GrantUserRole(UpdateUserParam param)
        {
            // 数据范围检查
            CheckDataScopeByUserId(param.Id);
            await _sysUserRoleService.GrantRoleAsync(param);
        }

        /// <inheritdoc/>
        [HttpPost("changeStatus")]
        public async Task ChangeUserStatus(UpdateUserParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (user.UserType == UserType.SuperAdmin)
                throw Oops.Oh(ErrorCode.E1015);

            if (!Enum.IsDefined(typeof(ValidityStatus), param.Status))
                throw Oops.Oh(ErrorCode.E3005);
            user.Status = param.Status;
            await _sysUserRep.AsUpdateable(user).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpPost("updateAvatar")]
        public async Task UpdateAvatar([FromQuery] UploadAvatarParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            user.Avatar = param.Avatar.ToString();
            await _sysUserRep.AsUpdateable(user).IgnoreColumns(it => new { it.UserType }).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateUser(UpdateUserParam param)
        {
            // 数据范围检查
            CheckDataScopeByUserId(param.Id);

            // 排除自己并且判断与其他是否相同
            var isExist = await _sysUserRep.AnyAsync(u => u.Account == param.Account && u.Id != param.Id);
            if (isExist) throw Oops.Oh(ErrorCode.E1003);

            var user = param.Adapt<SysUser>();

            try
            {
                _sysUserRep.Ado.BeginTran();
                await _sysUserRep.AsUpdateable(user).IgnoreColumns(it => new { it.Password, it.Status, it.UserType }).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
                param.SysEmpParam.Id = user.Id;
                // 更新员工及附属机构职位信息
                await _sysEmpService.AddOrUpdate(param.SysEmpParam);
                _sysUserRep.Ado.CommitTran();
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            }
            catch (Exception)
            {
                _sysUserRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        [HttpPost("updateInfo")]
        public async Task UpdateUserInfo(UpdateUserParam param)
        {
            var user = param.Adapt<SysUser>();
            await _sysUserRep.AsUpdateable(user)
                .IgnoreColumns(ignoreAllNullColumns: true)
                .IgnoreColumns(it => new { it.UserType, it.LastLoginTime })
                .ExecuteCommandAsync();
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
        }

        /// <inheritdoc/>
        [HttpPost("updatePwd")]
        public async Task UpdateUserPwd(UpdatePasswordUserParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (MD5Encryption.Encrypt(param.Password) != user.Password)
                throw Oops.Oh(ErrorCode.E1004);
            user.Password = MD5Encryption.Encrypt(param.NewPassword);
            await _sysUserRep.AsUpdateable(user).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpPost("resetPwd")]
        public async Task ResetUserPwd(QueryUserParam param)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            user.Password = MD5Encryption.Encrypt(await _sysConfigService.GetDefaultPasswordAsync());
            await _sysUserRep.AsUpdateable(user).IgnoreColumns(it => new { it.UserType }).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
        }
        #endregion
        #region NonAction
        /// <inheritdoc/>
        [NonAction]
        public async void CheckDataScope(long orgId)
        {
            // 如果当前用户不是管理员，则进行数据范围校验
            if (!AppUser.IsSuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList(AppUser.UserId);
                if (dataScopes == null || orgId != 0 || !dataScopes.Contains(orgId))
                    throw Oops.Oh(ErrorCode.E1013);
            }
        }
        /// <inheritdoc/>
        [NonAction]
        public async void CheckDataScopeByUserId(long userId)
        {
            // 如果当前用户不是超级管理员，则进行数据范围校验
            if (!AppUser.IsSuperAdmin)
            {
                var dataScopes = await GetDataScopeIdUserList(AppUser.UserId);
                if (dataScopes == null || userId == 0 || !dataScopes.Contains(userId))
                    throw Oops.Oh(ErrorCode.E1013);
            }
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<long>> GetDataScopeIdUserList(long userId)
        {
            userId = userId == 0 ? AppUser.UserId : userId;
            var list = await _sysCacheService.GetUsersDataScope(userId); // 先从缓存里面读取
            if (list == null || list.Count < 1)
            {
                var dataScopes = await GetUserDataScopeIdList(userId);
                list = (await _sysEmpService.HasOrgEmp(dataScopes)).Select(a => a.Id).ToList();
                list.Add(userId);
                list = list.Distinct().ToList();
                await _sysCacheService.SetUsersDataScope(userId, list); // 缓存结果
            }
            return list;
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<dynamic> GetUserById(long userId)
        {
            return await _sysUserRep.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList(long userId)
        {
            userId = userId == 0 ? AppUser.UserId : userId;
            var dataScopes = await _sysCacheService.GetDataScope(userId); // 先从缓存里面读取
            if (dataScopes == null || dataScopes.Count < 1)
            {
                if (!AppUser.IsSuperAdmin)
                {
                    var orgId = await _sysEmpService.GetEmpOrgId(userId);
                    // 获取该用户对应的数据范围集合
                    var userDataScopeIdListForUser = await _sysUserDataScopeService.GetUserDataScopeIdListAsync(userId);
                    // 获取该用户的角色对应的数据范围集合
                    if (orgId != null)
                    {
                        var userDataScopeIdListForRole = await _sysUserRoleService.GetUserRoleDataScopeIdListAsync(userId, (long)orgId);
                        dataScopes = userDataScopeIdListForUser.Concat(userDataScopeIdListForRole).Distinct().ToList(); // 并集 
                    }
                }
                else
                {
                    dataScopes = await _sysUserRep.AsQueryable().Select(u => u.Id).ToListAsync();
                }
                await _sysCacheService.SetDataScope(userId, dataScopes); // 缓存结果
            }
            return dataScopes;
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task SaveAuthUserToUser(AuthUserParam authUser, UserParam sysUser)
        {
            var user = sysUser.Adapt<SysUser>();
            user.UserType = UserType.User; // 非管理员

            // oauth账号与系统账号判断
            var isExist = await _sysUserRep.AnyAsync(u => u.Account == authUser.Username);
            user.Account = isExist ? authUser.Username + DateTime.Now.Ticks : authUser.Username;
            user.RealName = user.NickName = authUser.Nickname;
            user.Email = authUser.Email;
            user.Sex = authUser.Gender;
            await _sysUserRep.InsertAsync(user);
        }
        #endregion
    }
}
