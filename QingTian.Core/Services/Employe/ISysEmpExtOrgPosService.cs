namespace QingTian.Core.Services
{
    /// <summary>
     /// 员工附属机构和职位服务
     /// </summary>
    public interface ISysEmpExtOrgPosService
    {
        /// <summary>
        /// 保存或编辑附属机构相关信息
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="extIdList"></param>
        /// <returns></returns>
        Task AddOrUpdate(long empId, List<EmpExtOrgPosView> extIdList);
        /// <summary>
        /// 根据员工Id删除对应的员工-附属信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task DeleteEmpExtInfoByUserId(long empId);
        /// <summary>
        /// 获取附属机构和职位信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task<List<EmpExtOrgPosView>> GetEmpExtOrgPosList(long empId);
        /// <summary>
        /// 获取附属机构和职位信息
        /// </summary>
        /// <param name="empIds"></param>
        /// <returns></returns>
        Task<List<EmpExtOrgPosView>> GetEmpExtOrgPosList(List<long> empIds);
        /// <summary>
        /// 根据机构Id判断该附属机构下是否有员工
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<bool> HasExtOrgEmp(long orgId);
        /// <summary>
        /// 根据职位Id判断该附属职位下是否有员工
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<bool> HasExtPosEmp(long posId);
    }
}