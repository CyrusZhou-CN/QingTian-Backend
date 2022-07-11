using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public interface ISysEmpService
    {
        /// <summary>
        /// 增加或编辑员工相关信息
        /// </summary>
        /// <param name="sysEmpParam"></param>
        /// <returns></returns>
        Task AddOrUpdate(EmployeView2 sysEmpParam);
        /// <summary>
        /// 根据员工Id删除对应的员工表信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task DeleteEmpInfoByUserId(long empId);
        /// <summary>
        /// 获取用户员工相关信息（包括登录）
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task<EmployeView> GetEmpInfo(long empId);
        /// <summary>
        /// 获取用户员工相关信息列表
        /// </summary>
        /// <param name="empIds"></param>
        /// <returns></returns>
        Task<List<EmployeView>> GetEmpInfo(List<long> empIds);
        /// <summary>
        /// 获取员工机构Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task<long> GetEmpOrgId(long empId);
        /// <summary>
        /// 根据机构Id判断该机构下是否有员工
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<bool> HasOrgEmp(long orgId);
        /// <summary>
        /// 获取子机构用户
        /// </summary>
        /// <param name="orgIds"></param>
        /// <returns></returns>
        Task<List<SysEmploye>> HasOrgEmp(List<long> orgIds);
        /// <summary>
        /// 修改员工相关机构信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        Task UpdateEmpOrgInfo(long orgId, string orgName);
    }
}
