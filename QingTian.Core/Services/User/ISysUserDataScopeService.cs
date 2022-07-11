namespace QingTian.Core.Services
{
    /// <summary>
    /// 用户数据范围服务
    /// </summary>
    public interface ISysUserDataScopeService
    {
        /// <summary>
        /// 根据机构Id集合删除对应的用户-数据范围关联信息
        /// </summary>
        /// <param name="orgIdList"></param>
        /// <returns></returns>
        Task DeleteUserDataScopeListByOrgIdListAsync(List<long> orgIdList);
        /// <summary>
        /// 根据用户Id删除对应的用户-数据范围关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserDataScopeListByUserIdAsync(long userId);
        /// <summary>
        /// 获取用户的数据范围Id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> GetUserDataScopeIdListAsync(long userId);
        /// <summary>
        /// 授权用户数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantDataAsync(UpdateUserParam param);
    }
}