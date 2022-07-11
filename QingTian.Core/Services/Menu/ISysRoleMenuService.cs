using Microsoft.AspNetCore.Mvc;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public interface ISysRoleMenuService
    {
        /// <summary>
        /// 根据菜单Id集合删除对应的角色-菜单表信息
        /// </summary>
        /// <param name="menuIdList"></param>
        /// <returns></returns>
        Task DeleteRoleMenuListByMenuIdList(List<long> menuIdList);
        /// <summary>
        /// 根据角色Id删除对应的角色-菜单表关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task DeleteRoleMenuListByRoleId(long roleId);
        /// <summary>
        /// 获取角色的菜单Id集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        Task<List<long>> GetRoleMenuIdList(List<long> roleIdList);
        /// <summary>
        /// 授权角色菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantMenu(GrantRoleMenuParam param);
        /// <summary>
        /// 清理角色权限
        /// </summary>
        /// <param name="manageRoleId"></param>
        /// <returns></returns>
        Task ClearRoleMenuListByTenantId(long manageRoleId);
    }
}