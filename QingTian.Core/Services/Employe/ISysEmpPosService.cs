namespace QingTian.Core.Services
{

    /// <summary>
    /// 员工职位服务
    /// </summary>
    public interface ISysEmpPosService
    {
        /// <summary>
        /// 增加或编辑员工职位相关信息
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="posIdList"></param>
        /// <returns></returns>
        Task AddOrUpdate(long empId, List<long> posIdList);
        /// <summary>
        /// 删除员工职位相关信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task DeleteEmpPosInfoByUserId(long empId);
        /// <summary>
        /// 获取所属职位信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task<List<EmpPosView>> GetEmpPosList(long empId);
        /// <summary>
        /// 获取所属职位信息
        /// </summary>
        /// <param name="empIds"></param>
        /// <returns></returns>
        Task<List<EmpPosView>> GetEmpPosList(List<long> empIds);
        /// <summary>
        /// 根据职位Id判断该职位下是否有员工
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<bool> HasPosEmp(long posId);
    }
}