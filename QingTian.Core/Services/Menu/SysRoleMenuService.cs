using Furion.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.Menu
{

    /// <inheritdoc cref="ISysRoleMenuService"/>
    public class SysRoleMenuService : ISysRoleMenuService, ITransient
    {
        private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenuRep;  // 角色菜单表
        private readonly ISysCacheService _sysCacheService;
        private readonly SqlSugarRepository<SysRole> _sysRoleRep;

        public SysRoleMenuService(SqlSugarRepository<SysRoleMenu> sysRoleMenuRep, ISysCacheService sysCacheService, SqlSugarRepository<SysRole> sysRoleRep)
        {
            _sysRoleMenuRep = sysRoleMenuRep;
            _sysCacheService = sysCacheService;
            _sysRoleRep = sysRoleRep;
        }

        /// <inheritdoc/>
        public async Task ClearRoleMenuListByTenantId(long manageRoleId)
        {
            List<long> roleIds = await _sysRoleRep.Where(m => m.Status == ValidityStatus.ENABLE).Select(m => m.Id).ToListAsync();
            roleIds.Remove(manageRoleId);
            var managePermissionList = await _sysRoleMenuRep.Where(m => m.SysRoleId == manageRoleId).Select(m => m.SysMenuId).ToListAsync();

            await _sysRoleMenuRep.DeleteAsync(m => roleIds.Contains(m.SysRoleId) && !managePermissionList.Contains(m.SysMenuId));
        }

        /// <inheritdoc/>
        public async Task DeleteRoleMenuListByMenuIdList(List<long> menuIdList)
        {
            await _sysRoleMenuRep.DeleteAsync(u => menuIdList.Contains(u.SysMenuId));
        }


        /// <inheritdoc/>
        public async Task DeleteRoleMenuListByRoleId(long roleId)
        {
            await _sysRoleMenuRep.DeleteAsync(u => u.SysRoleId == roleId);
        }

        /// <inheritdoc/>
        public async Task<List<long>> GetRoleMenuIdList(List<long> roleIdList)
        {
            return await _sysRoleMenuRep
                                        .Where(u => roleIdList.Contains(u.SysRoleId))
                                        .Select(u => u.SysMenuId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task GrantMenu(GrantRoleMenuParam param)
        {
            try
            {
                _sysRoleMenuRep.Ado.BeginTran();
                await _sysRoleMenuRep.DeleteAsync(u => u.SysRoleId == param.Id);

                var grantMenuIdList = new List<SysRoleMenu>();

                param.GrantMenuIdList.ForEach(u =>
                {
                    grantMenuIdList.Add(
                    new SysRoleMenu
                    {
                        SysRoleId = param.Id,
                        SysMenuId = u
                    });
                });
                await _sysRoleMenuRep.InsertAsync(grantMenuIdList);
                _sysRoleMenuRep.Ado.CommitTran();
            }
            catch (System.Exception)
            {
                _sysRoleMenuRep.Ado.RollbackTran();
                throw;
            }
            // 清除缓存
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_MENU);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_PERMISSION);
        }
    }
}
