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
    /// <inheritdoc cref="ISysEmpPosService"/>
    public class SysEmpPosService : ISysEmpPosService, ITransient
    {
        private readonly SqlSugarRepository<SysEmpPos> _sysEmpPosRep;  // 员工职位表

        public SysEmpPosService(SqlSugarRepository<SysEmpPos> sysEmpPosRep)
        {
            _sysEmpPosRep = sysEmpPosRep;
        }

        /// <inheritdoc/>
        public async Task AddOrUpdate(long empId, List<long> posIdList)
        {
            try
            {
                _sysEmpPosRep.Ado.BeginTran();
                // 先删除
                await DeleteEmpPosInfoByUserId(empId);

                if (posIdList != null && posIdList.Any())
                {
                    List<SysEmpPos> list = new List<SysEmpPos>();
                    posIdList.ForEach(u =>
                    {
                        list.Add(new SysEmpPos
                        {
                            SysEmpId = empId,
                            SysPosId = u
                        });
                    });
                    await _sysEmpPosRep.InsertAsync(list);
                }
                _sysEmpPosRep.Ado.CommitTran();
            }
            catch (System.Exception ex)
            {
                _sysEmpPosRep.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteEmpPosInfoByUserId(long empId)
        {
            await _sysEmpPosRep.DeleteAsync(u => u.SysEmpId == empId);
        }

        /// <inheritdoc/>
        public async Task<List<EmpPosView>> GetEmpPosList(long empId)
        {
            return await _sysEmpPosRep.Context.Queryable<SysEmpPos, SysPosition>((e, p) => new JoinQueryInfos(
                 JoinType.Inner, e.SysPosId == p.Id
                 ))
                                      .Where((e, p) => e.SysEmpId == empId)
                                      .Select((e, p) => new EmpPosView
                                      {
                                          PosId = p.Id,
                                          PosCode = p.Code,
                                          PosName = p.Name
                                      }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<EmpPosView>> GetEmpPosList(List<long> empIds)
        {
            return await _sysEmpPosRep.Context.Queryable<SysEmpPos, SysPosition>((e, p) => new JoinQueryInfos(
                 JoinType.Inner, e.SysPosId == p.Id
                 ))
                                      .Where((e, p) => empIds.Contains(e.SysEmpId))
                                      .Select((e, p) => new EmpPosView
                                      {
                                          PosId = p.Id,
                                          PosCode = p.Code,
                                          PosName = p.Name
                                      }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> HasPosEmp(long posId)
        {
            return await _sysEmpPosRep.AnyAsync(u => u.SysPosId == posId);
        }
    }
}
