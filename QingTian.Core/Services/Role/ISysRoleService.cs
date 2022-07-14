using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 系统角色服务
    /// </summary>
    public interface ISysRoleService
    {
        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task<bool> SetRoleStatus(long Id, int newStatus);
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddRole(AddRoleParam param);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteRole(DeleteRoleParam param);

        /// <summary>
        /// 根据角色Id获取角色名称
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<string> GetNameByRoleId(long roleId);

        /// <summary>
        /// 角色下拉（用于授权角色时选择）
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetRoleDropDown();

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysRole> GetRoleInfo([FromQuery] QueryRoleParam param);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetRoleList([FromQuery] RoleParam param);

        /// <summary>
        /// 根据角色Id集合获取数据范围Id集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<List<long>> GetUserDataScopeIdList(List<long> roleIdList, long orgId);

        /// <summary>
        /// 获取用户角色相关信息（登录）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<RoleView>> GetUserRoleList(long userId);

        /// <summary>
        /// 授权角色数据范围
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantData(GrantRoleDataParam param);

        /// <summary>
        /// 授权角色菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantMenu(GrantRoleMenuParam param);

        /// <summary>
        /// 获取角色拥有数据Id集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<long>> OwnData([FromQuery] QueryRoleParam param);

        /// <summary>
        /// 获取角色拥有菜单Id集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<OwnMenuView>> OwnMenu([FromQuery] QueryRoleParam param);

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryRolePageList([FromQuery] RoleParam param);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateRole(UpdateRoleParam param);
    }
}
