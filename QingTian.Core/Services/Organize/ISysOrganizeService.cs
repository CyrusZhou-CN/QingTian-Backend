using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    public interface ISysOrganizeService
    {

        /// <summary>
        /// 增加组织机构
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddOrganize(AddOrganizeParam param);

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteOrganize(DeleteOrganizeParam param);

        /// <summary>
        /// 根据数据范围类型获取当前用户的数据范围（机构Id）集合
        /// </summary>
        /// <param name="dataScopeType"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId);

        /// <summary>
        /// 获取组织机构信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysOrganize> GetOrganize([FromQuery] QueryOrganizeParam param);

        /// <summary>
        /// 获取组织机构列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<OrganizeView>> GetOrganizeList([FromQuery] OrganizeParam param);

        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        Task<dynamic> GetOrganizeTree([FromQuery] OrganizeParam Param);

        /// <summary>
        /// 分页查询组织机构
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryOrganizePageList([FromQuery] PageOrganizeParam param);
        Task UpdateOrganize(UpdateOrganizeParam param);
        /// <summary>
        /// 获取所有的机构组织Id集合
        /// </summary>
        /// <returns></returns>
        Task<List<long>> GetAllDataScopeIdList();
    }
}