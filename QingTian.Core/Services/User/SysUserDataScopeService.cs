using Furion.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysUserDataScopeService"/>   
    public class SysUserDataScopeService : ISysUserDataScopeService, ITransient
    {
        private readonly SqlSugarRepository<SysUserDataScope> _sysUserDataScopeRep;  // 用户数据范围表 

        /// <inheritdoc/>   
        public SysUserDataScopeService(SqlSugarRepository<SysUserDataScope> sysUserDataScopeRep)
        {
            _sysUserDataScopeRep = sysUserDataScopeRep;
        }
        /// <inheritdoc/>        
        public async Task DeleteUserDataScopeListByOrgIdListAsync(List<long> orgIdList)
        {
            await _sysUserDataScopeRep.DeleteAsync(u => orgIdList.Contains(u.SysOrgId));
        }

        /// <inheritdoc/>   
        public async Task DeleteUserDataScopeListByUserIdAsync(long userId)
        {
            await _sysUserDataScopeRep.DeleteAsync(u => u.SysUserId == userId);
        }

        /// <inheritdoc/>   
        public async Task<List<long>> GetUserDataScopeIdListAsync(long userId)
        {
            return await _sysUserDataScopeRep.Where(u => u.SysUserId == userId)
                                              .Select(u => u.SysOrgId).ToListAsync();
        }

        /// <inheritdoc/>   
        public async Task GrantDataAsync(UpdateUserParam param)
        {
            try
            {
                _sysUserDataScopeRep.Ado.BeginTran();
                await _sysUserDataScopeRep.DeleteAsync(u => u.SysUserId == param.Id);
                var grantOrgIdList = new List<SysUserDataScope>();
                param.GrantOrgIdList.ForEach(u =>
                {
                    grantOrgIdList.Add(
                    new SysUserDataScope
                    {
                        SysUserId = param.Id,
                        SysOrgId = u
                    });
                });
                await _sysUserDataScopeRep.InsertAsync(grantOrgIdList);
                _sysUserDataScopeRep.Ado.CommitTran();
            }
            catch (System.Exception ex)
            {
                _sysUserDataScopeRep.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}
