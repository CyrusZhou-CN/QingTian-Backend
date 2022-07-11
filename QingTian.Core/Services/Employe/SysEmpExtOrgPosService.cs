using Furion.DependencyInjection;
using QingTian.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysEmpExtOrgPosService"/>
    public class SysEmpExtOrgPosService : ISysEmpExtOrgPosService, ITransient
    {
        private readonly SqlSugarRepository<SysEmpExtOrgPos> _sysEmpExtOrgPosRep;  // 附属机构表

        public SysEmpExtOrgPosService(SqlSugarRepository<SysEmpExtOrgPos> sysEmpExtOrgPosRep)
        {
            _sysEmpExtOrgPosRep = sysEmpExtOrgPosRep;
        }

        /// <inheritdoc/>
        public async Task AddOrUpdate(long empId, List<EmpExtOrgPosView> extIdList)
        {
            try
            {
                _sysEmpExtOrgPosRep.Ado.BeginTran();
                // 先删除
                await DeleteEmpExtInfoByUserId(empId);

                if (extIdList != null && extIdList.Any())
                {
                    var tasks = new List<SysEmpExtOrgPos>();
                    extIdList.ForEach(u =>
                    {
                        tasks.Add(new SysEmpExtOrgPos
                        {
                            SysEmpId = empId,
                            SysOrgId = u.OrgId,
                            SysPosId = u.PosId
                        });
                    });
                    await _sysEmpExtOrgPosRep.InsertAsync(tasks);
                }
                _sysEmpExtOrgPosRep.Ado.CommitTran();
            }
            catch (System.Exception ex)
            {
                _sysEmpExtOrgPosRep.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteEmpExtInfoByUserId(long empId)
        {
            await _sysEmpExtOrgPosRep.DeleteAsync(u => u.SysEmpId == empId);
        }

        /// <inheritdoc/>
        public async Task<List<EmpExtOrgPosView>> GetEmpExtOrgPosList(long empId)
        {
            return await _sysEmpExtOrgPosRep.Context.Queryable<SysEmpExtOrgPos, SysPosition, SysOrganize>((e, p, o) => new JoinQueryInfos(
                 JoinType.Inner, e.SysPosId == p.Id,
                 JoinType.Inner, e.SysOrgId == o.Id
                 ))
                                           .Where((e, p, o) => e.SysEmpId == empId)
                                           .Select((e, p, o) => new EmpExtOrgPosView
                                           {
                                               SysEmpId = e.SysEmpId,
                                               OrgId = o.Id,
                                               OrgName = o.Name,
                                               PosId = p.Id,
                                               PosCode = p.Code,
                                               PosName = p.Name
                                           }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<EmpExtOrgPosView>> GetEmpExtOrgPosList(List<long> empIds)
        {
            return await _sysEmpExtOrgPosRep.Context.Queryable<SysEmpExtOrgPos, SysPosition, SysOrganize>((e, p, o) => new JoinQueryInfos(
                  JoinType.Inner, e.SysPosId == p.Id,
                  JoinType.Inner, e.SysOrgId == o.Id
                  ))
                                            .Where((e, p, o) => empIds.Contains(e.SysEmpId))
                                            .Select((e, p, o) => new EmpExtOrgPosView
                                            {
                                                OrgId = o.Id,
                                                OrgName = o.Name,
                                                PosId = p.Id,
                                                PosCode = p.Code,
                                                PosName = p.Name
                                            }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> HasExtOrgEmp(long orgId)
        {
            return await _sysEmpExtOrgPosRep.AnyAsync(u => u.SysOrgId == orgId);
        }

        /// <inheritdoc/>
        public async Task<bool> HasExtPosEmp(long posId)
        {
            return await _sysEmpExtOrgPosRep.AnyAsync(u => u.SysPosId == posId);
        }
    }
}
