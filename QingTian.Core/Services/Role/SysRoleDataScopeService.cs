using Furion.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 角色数据范围服务
    /// </summary>
    public class SysRoleDataScopeService : ISysRoleDataScopeService, ITransient
    {
        private readonly SqlSugarRepository<SysRoleDataScope> _sysRoleDataScopeRep;  // 角色数据范围表 
        private readonly SqlSugarRepository<SysRole> _roleRep;

        public SysRoleDataScopeService(SqlSugarRepository<SysRoleDataScope> sysRoleDataScopeRep, SqlSugarRepository<SysRole> roleRep)
        {
            _sysRoleDataScopeRep = sysRoleDataScopeRep;
            _roleRep = roleRep;
        }

        /// <summary>
        /// 根据机构Id集合删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="orgIdList"></param>
        /// <returns></returns>
        public async Task DeleteRoleDataScopeListByOrgIdList(List<long> orgIdList)
        {
            await _sysRoleDataScopeRep.DeleteAsync(u => orgIdList.Contains(u.SysOrgId));
        }

        /// <summary>
        /// 根据角色Id删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteRoleDataScopeListByRoleId(long roleId)
        {
            await _sysRoleDataScopeRep.DeleteAsync(u => u.SysRoleId == roleId);
        }

        /// <summary>
        /// 根据角色Id集合获取角色数据范围集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        public async Task<List<long>> GetRoleDataScopeIdList(List<long> roleIdList)
        {
            return await _sysRoleDataScopeRep
                                             .Where(u => roleIdList.Contains(u.SysRoleId))
                                             .Select(u => u.SysOrgId).ToListAsync();
        }

        /// <summary>
        /// 授权角色数据范围
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task GrantDataScope(GrantRoleDataParam param)
        {
            try
            {
                _sysRoleDataScopeRep.Ado.BeginTran();
                await _sysRoleDataScopeRep.DeleteAsync(u => u.SysRoleId == param.Id);

                var grantOrgIdList = new List<SysRoleDataScope>();
                param.GrantOrgIdList.ForEach(u =>
                {
                    grantOrgIdList.Add(
                    new SysRoleDataScope
                    {
                        SysRoleId = param.Id,
                        SysOrgId = u
                    });
                });
                await _roleRep.AsUpdateable(new SysRole() { DataScopeType = param.DataScopeType })
                    .Where(m => m.Id == param.Id)
                    .ExecuteCommandAsync();
                await _sysRoleDataScopeRep.InsertAsync(grantOrgIdList);
                _sysRoleDataScopeRep.Ado.CommitTran();
            }
            catch (System.Exception ex)
            {
                _sysRoleDataScopeRep.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}
