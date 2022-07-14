using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using QingTian.Core.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="ISysRoleService"/>
    [Route("SysRole"), ApiDescriptionSettings(Name = "Role", Order = 998)]
    public class SysRoleService : ISysRoleService, IDynamicApiController, ITransient
    {

        private readonly SqlSugarRepository<SysRole> _sysRoleRep;  // 角色表
        private readonly SqlSugarRepository<SysUserRole> _sysUserRoleRep;  // 用户角色表
        private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenu;

        private readonly ISysRoleDataScopeService _sysRoleDataScopeService;
        private readonly ISysOrganizeService _sysOrgService;
        private readonly ISysRoleMenuService _sysRoleMenuService;
        private readonly ISysCacheService _sysCacheService;

        public SysRoleService(SqlSugarRepository<SysRole> sysRoleRep,
                              SqlSugarRepository<SysUserRole> sysUserRoleRep,
                              SqlSugarRepository<SysRoleMenu> sysRoleMenu,
                              ISysRoleDataScopeService sysRoleDataScopeService,
                              ISysOrganizeService sysOrgService,
                              ISysRoleMenuService sysRoleMenuService,
                              ISysCacheService sysCacheService)
        {
            _sysRoleRep = sysRoleRep;
            _sysUserRoleRep = sysUserRoleRep;
            _sysRoleMenu = sysRoleMenu;
            _sysRoleDataScopeService = sysRoleDataScopeService;
            _sysOrgService = sysOrgService;
            _sysRoleMenuService = sysRoleMenuService;
            _sysCacheService = sysCacheService;
        }


        /// <inheritdoc/>
        [HttpPost("setRoleStatus")]
        public async Task<bool> SetRoleStatus(long Id, int newStatus)
        {
            var role = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == Id); 
            if (role.IsNullOrZero())
                throw Oops.Oh(ErrorCode.E1002);
            role.Status = (ValidityStatus)newStatus;
            await _sysRoleRep.AsUpdateable(role).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
            return true;

        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddRole(AddRoleParam param)
        {
            var isExist = await _sysRoleRep.AnyAsync(u => u.Code == param.Code || u.Name == param.Name);
            if (isExist)
                throw Oops.Oh(ErrorCode.E1006);

            var role = param.Adapt<SysRole>();
            role.DataScopeType = DataScopeType.ALL; // 新角色默认全部数据范围
            await _sysRoleRep.InsertAsync(role);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteRole(DeleteRoleParam param)
        {

            var sysRole = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (sysRole.IsNullOrZero())
                throw Oops.Oh(ErrorCode.E1002);
            try
            {
                _sysRoleRep.Ado.BeginTran();
                await _sysRoleRep.DeleteAsync(sysRole);
                //级联删除该角色对应的角色-数据范围关联信息
                await _sysRoleDataScopeService.DeleteRoleDataScopeListByRoleId(sysRole.Id);
                ////级联删除该角色对应的用户-角色表关联信息
                await _sysUserRoleRep.DeleteAsync(u => u.SysRoleId == sysRole.Id);
                //级联删除该角色对应的角色-菜单表关联信息
                await _sysRoleMenuService.DeleteRoleMenuListByRoleId(sysRole.Id);
                _sysRoleRep.Ado.CommitTran();
            }
            catch (Exception)
            {
                _sysRoleRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<string> GetNameByRoleId(long roleId)
        {
            var role = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == roleId);
            if (role == null)
                throw Oops.Oh(ErrorCode.E1002);
            return role.Name;
        }

        /// <inheritdoc/>
        [HttpGet("dropDown")]
        public async Task<dynamic> GetRoleDropDown()
        {
            // 如果不是超级管理员，则查询自己拥有的角色集合
            var roles = AppUser.IsSuperAdmin
                        ? await _sysUserRoleRep.Where(u => u.SysUserId == AppUser.UserId).Select(u => u.SysRoleId).ToListAsync()
                        : new List<long>();

            return await _sysRoleRep
                                    .Where(roles.Any(), u => roles.Contains(u.Id))
                                    .Where(u => u.Status == (int)ValidityStatus.ENABLE)
                                    .Select(u => new
                                    {
                                        u.Id,
                                        u.Code,
                                        u.Name
                                    }).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysRole> GetRoleInfo([FromQuery] QueryRoleParam param)
        {
            return await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<dynamic> GetRoleList([FromQuery] RoleParam param)
        {

            return await _sysRoleRep.AsQueryable()
                                   .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                                   .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                                   .Where(u => u.Status == (int)ValidityStatus.ENABLE).OrderBy(u => u.Sort)
                                   .Select(u => new
                                   {
                                       u.Id,
                                       Name = u.Name + "[" + u.Code + "]"
                                   }).ToListAsync();
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList(List<long> roleIdList, long orgId)
        {

            // 定义角色中最大数据范围的类型，目前按最大范围策略来，如果你同时拥有ALL和SELF的权限，最后按ALL返回
            int strongerDataScopeType = (int)DataScopeType.SELF;

            var customDataScopeRoleIdList = new List<long>();
            if (roleIdList != null && roleIdList.Count > 0)
            {
                var roles = await _sysRoleRep.Where(u => roleIdList.Contains(u.Id)).ToListAsync();
                roles.ForEach(u =>
                {
                    if (u.DataScopeType == DataScopeType.DEFINE)
                        customDataScopeRoleIdList.Add(u.Id);
                    else if ((int)u.DataScopeType <= strongerDataScopeType)
                        strongerDataScopeType = (int)u.DataScopeType;
                });
            }

            // 自定义数据范围的角色对应的数据范围
            var roleDataScopeIdList = await _sysRoleDataScopeService.GetRoleDataScopeIdList(customDataScopeRoleIdList);

            // 角色中拥有最大数据范围类型的数据范围
            var dataScopeIdList = await _sysOrgService.GetDataScopeListByDataScopeType(strongerDataScopeType, orgId);

            return roleDataScopeIdList.Concat(dataScopeIdList).Distinct().ToList(); //并集
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<RoleView>> GetUserRoleList(long userId)
        {
            return await _sysRoleRep
                .Context
                .Queryable<SysRole, SysUserRole>((r, u) =>
                new JoinQueryInfos(JoinType.Inner, r.Id == u.SysRoleId))
                .Where((r, u) => u.SysUserId == userId)
                .Select((r, u) => new RoleView()
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name
                }).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpPost("grantData")]
        public async Task GrantData(GrantRoleDataParam param)
        {

            // 清除所有用户数据范围缓存
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);

            var role = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            var dataScopeType = param.DataScopeType;
            if (!AppUser.IsSuperAdmin)
            {
                //如果授权的角色数据范围类型为自定义，则要判断授权的数据范围是否在自己的数据范围内
                if (DataScopeType.DEFINE == dataScopeType)
                {
                    var dataScopes = await DataFilterExtensions.GetDataScopeIdList(FilterType.Org);
                    var grantOrgIdList = param.GrantOrgIdList; //要授权的数据范围列表
                    if (grantOrgIdList.Count > 0)
                    {
                        if (dataScopes.Count < 1)
                            throw Oops.Oh(ErrorCode.E1016);
                        else if (!dataScopes.All(u => grantOrgIdList.Any(c => c == u)))
                            throw Oops.Oh(ErrorCode.E1016);
                    }
                }
            }
            role.DataScopeType = dataScopeType;
            await _sysRoleDataScopeService.GrantDataScope(param);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);
        }

        /// <inheritdoc/>
        [HttpPost("grantMenu")]
        public async Task GrantMenu(GrantRoleMenuParam param)
        {
            await _sysRoleMenuService.GrantMenu(param);
        }

        /// <inheritdoc/>
        [HttpGet("ownData")]
        public async Task<List<long>> OwnData([FromQuery] QueryRoleParam param)
        {
            return await _sysRoleDataScopeService.GetRoleDataScopeIdList(new List<long> { param.Id });
        }

        /// <inheritdoc/>
        [HttpGet("ownMenu")]
        public async Task<List<OwnMenuView>> OwnMenu([FromQuery] QueryRoleParam param)
        {

            var menuList = await _sysRoleMenu.AsQueryable().LeftJoin<SysMenu>((t1, t2) => t1.SysMenuId == t2.Id)
                .Where(t1 => t1.SysRoleId == param.Id)
                .Select((t1, t2) => new SysMenu { Id = t1.SysMenuId }).ToListAsync();

            return menuList.Select(m => new OwnMenuView
            {
                SysRoleId = param.Id,
                MenuIdList = menuList.Select(sl => sl.Id).ToList()
            }).ToList();
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryRolePageList([FromQuery] RoleParam param)
        {

            var roles = await _sysRoleRep.AsQueryable()
                                         .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                                         .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                                         .OrderBy(u => u.Sort)
                                         .ToPagedListAsync(param.Page, param.PageSize);
            return roles.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateRole(UpdateRoleParam param)
        {
            var role = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (role.IsNullOrZero())
                throw Oops.Oh(ErrorCode.E1002);
            if (await _sysRoleRep.IsExistsAsync(u => (u.Name == param.Name || u.Code == param.Code) && u.Id != param.Id))
                throw Oops.Oh(ErrorCode.E1006);
            var sysRole = param.Adapt<SysRole>();
            await _sysRoleRep.AsUpdateable(sysRole).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
    }
}
