using Furion.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysUserRoleService"/> 
    public class SysUserRoleService : ISysUserRoleService, ITransient
    {
        private readonly SqlSugarRepository<SysUserRole> _sysUserRoleRep;  // 用户权限数据表

        private readonly ISysRoleService _sysRoleService;

        public SysUserRoleService(SqlSugarRepository<SysUserRole> sysUserRoleRep, ISysRoleService sysRoleService)
        {
            _sysUserRoleRep = sysUserRoleRep;
            _sysRoleService = sysRoleService;
        }
        /// <inheritdoc/> 
        public async Task<int> DeleteUserRoleListByRoleIdAsync(long roleId)
        {
            return await _sysUserRoleRep.DeleteAsync(u => u.SysRoleId == roleId);
        }
        /// <inheritdoc/> 
        public async Task<int> DeleteUserRoleListByUserIdAsync(long userId)
        {
            return await _sysUserRoleRep.DeleteAsync(u => u.SysUserId == userId);
        }
        /// <inheritdoc/> 
        public async Task<List<long>> GetUserRoleDataScopeIdListAsync(long userId, long orgId)
        {
            var roleIdList = await GetUserRoleIdListAsync(userId);
            // 获取这些角色对应的数据范围
            if (roleIdList.Count > 0)
                return await _sysRoleService.GetUserDataScopeIdList(roleIdList, orgId);

            return roleIdList;
        }
        /// <inheritdoc/> 
        public async Task<List<long>> GetUserRoleIdListAsync(long userId)
        {
            return await _sysUserRoleRep
                .Where(u => u.SysUserId == userId)
                .Select(u => u.SysRoleId).ToListAsync();
        }
        /// <inheritdoc/> 
        public async Task GrantRoleAsync(UpdateUserParam param)
        {
            try
            {
                _sysUserRoleRep.Ado.BeginTran();
                await _sysUserRoleRep
                    .DeleteAsync(u => u.SysUserId == param.Id);
                var grantRoleIdList = new List<SysUserRole>();
                param.GrantRoleIdList.ForEach(roleId =>
                {
                    grantRoleIdList.Add(
                    new SysUserRole
                    {
                        SysUserId = param.Id,
                        SysRoleId = roleId
                    });
                });
                await _sysUserRoleRep.InsertAsync(grantRoleIdList);
                _sysUserRoleRep.Ado.CommitTran();
            }
            catch (System.Exception ex)
            {
                _sysUserRoleRep.Ado.RollbackTran();
                throw ex;
            }
        }

    }
}
