using Furion.DependencyInjection;
using Mapster;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysEmpService"/>
    public class SysEmpService : ISysEmpService, ITransient
    {
        private readonly SqlSugarRepository<SysEmploye> _sysEmpRep;  // 员工表

        private readonly ISysEmpExtOrgPosService _sysEmpExtOrgPosService;
        private readonly ISysEmpPosService _sysEmpPosService;

        public SysEmpService(SqlSugarRepository<SysEmploye> sysEmpRep,
                             ISysEmpExtOrgPosService sysEmpExtOrgPosService,
                             ISysEmpPosService sysEmpPosService)
        {
            _sysEmpRep = sysEmpRep;
            _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
            _sysEmpPosService = sysEmpPosService;
        }

        /// <inheritdoc/>
        public async Task AddOrUpdate(EmployeView2 sysEmpParam)
        {
            try
            {
                _sysEmpRep.Ado.BeginTran();
                // 先删除员工信息
                await _sysEmpRep.DeleteAsync(u => u.Id == sysEmpParam.Id);

                // 再新增新员工信息
                var emp = sysEmpParam.Adapt<SysEmploye>();
                await _sysEmpRep.InsertAsync(emp);

                // 更新附属机构职位信息
                await _sysEmpExtOrgPosService.AddOrUpdate(emp.Id, sysEmpParam.ExtIds);

                // 更新职位信息
                await _sysEmpPosService.AddOrUpdate(emp.Id, sysEmpParam.PosIdList);
                _sysEmpRep.Ado.CommitTran();
            }
            catch (System.Exception)
            {
                _sysEmpRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteEmpInfoByUserId(long empId)
        {
            try
            {
                _sysEmpRep.Ado.BeginTran();

                var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId);
                // 级联删除对应的员工-附属信息
                await _sysEmpExtOrgPosService.DeleteEmpExtInfoByUserId(empId);

                // 级联删除对用的员工-职位信息
                await _sysEmpPosService.DeleteEmpPosInfoByUserId(empId);
                // 删除员工信息
                if (emp != null)
                {
                    await _sysEmpRep.DeleteAsync(emp);
                }
                _sysEmpRep.Ado.CommitTran();
            }
            catch (System.Exception)
            {
                _sysEmpRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EmployeView> GetEmpInfo(long empId)
        {
            var empInfoOutput = new EmployeView();
            var sysEmp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId);
            if (sysEmp == null) return empInfoOutput;

            empInfoOutput = sysEmp.Adapt<EmployeView>();
            empInfoOutput.ExtOrgPos = await _sysEmpExtOrgPosService.GetEmpExtOrgPosList(empId);
            empInfoOutput.Positions = await _sysEmpPosService.GetEmpPosList(empId);
            return empInfoOutput;
        }

        /// <inheritdoc/>
        public async Task<List<EmployeView>> GetEmpInfo(List<long> empIds)
        {
            List<EmployeView> empInfoOutputs = new List<EmployeView>();
            List<SysEmploye> sysEmps = await _sysEmpRep.Where(m => empIds.Contains(m.Id)).ToListAsync();
            if (sysEmps == null || !sysEmps.Any()) return empInfoOutputs;
            empInfoOutputs = sysEmps.Adapt<List<EmployeView>>();

            var extOrgPoses = await _sysEmpExtOrgPosService.GetEmpExtOrgPosList(empIds);
            var positions = await _sysEmpPosService.GetEmpPosList(empIds);

            foreach (var empInfoOutput in empInfoOutputs)
            {
                empInfoOutput.ExtOrgPos = extOrgPoses.Where(m => m.SysEmpId == empInfoOutput.Id).ToList();
                empInfoOutput.Positions = positions.Where(m => m.SysEmpId == empInfoOutput.Id).ToList();
            }
            return empInfoOutputs;
        }

        /// <inheritdoc/>
        public async Task<long?> GetEmpOrgId(long empId)
        {
            var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId);
            return emp?.OrgId;
        }

        /// <inheritdoc/>
        public async Task<bool> HasOrgEmp(long orgId)
        {
            return await _sysEmpRep.AnyAsync(u => u.OrgId == orgId);
        }

        /// <inheritdoc/>
        public async Task<List<SysEmploye>> HasOrgEmp(List<long> orgIds)
        {
            return await _sysEmpRep.Where(u => orgIds.Contains(u.OrgId)).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateEmpOrgInfo(long orgId, string orgName)
        {
            var emps = await _sysEmpRep.Where(u => u.OrgId == orgId).ToListAsync();
            emps.ForEach(u =>
            {
                u.OrgName = orgName;
            });
            await _sysEmpRep.UpdateAsync(emps);
        }
    }
}
