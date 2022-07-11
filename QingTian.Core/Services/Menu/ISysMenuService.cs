using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 系统菜单服务
    /// </summary>
    public interface ISysMenuService
    {
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddMenu(AddMenuParam param);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteMenu(DeleteMenuParam param);

        /// <summary>
        /// 用户菜单
        /// </summary>
        /// <returns></returns>
        Task<List<MenuTreeNode>> GetUserMenu();
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateMenu(UpdateMenuParam param);
        /// <summary>
        /// 获取用户AntDesign菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuTreeNode>> GetLoginMenusAntDesign(long userId);
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetLoginPermissionList(long userId);
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetMenu(QueryMenuParam param);
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuView>> GetMenuList([FromQuery] MenuParam param);
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MenuTreeView>> GetMenuTree([FromQuery] MenuParam param);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllPermission();
    }
}
