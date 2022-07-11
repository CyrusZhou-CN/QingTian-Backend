namespace QingTian.Core.Services
{
    /// <summary>
    /// 角色数据范围服务
    /// </summary>
    public interface ISysRoleDataScopeService
    {

        /// <summary>
        /// 根据机构Id集合删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="orgIdList"></param>
        /// <returns></returns>
        Task DeleteRoleDataScopeListByOrgIdList(List<long> orgIdList);

        /// <summary>
        /// 根据角色Id删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task DeleteRoleDataScopeListByRoleId(long roleId);

        /// <summary>
        /// 根据角色Id集合获取角色数据范围集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        Task<List<long>> GetRoleDataScopeIdList(List<long> roleIdList);

        /// <summary>
        /// 授权角色数据范围
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantDataScope(GrantRoleDataParam param);
    }
}