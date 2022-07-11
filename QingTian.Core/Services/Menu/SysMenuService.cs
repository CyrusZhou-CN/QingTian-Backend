using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="ISysMenuService"/>
    [Route("SysMenu"), ApiDescriptionSettings(Name = "Menu", Order = 0)]
    public class SysMenuService : ISysMenuService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysMenu> _sysMenuRep;  // 菜单表仓 

        private readonly ISysCacheService _sysCacheService;
        private readonly ISysUserRoleService _sysUserRoleService;
        private readonly ISysRoleMenuService _sysRoleMenuService;

        public SysMenuService(SqlSugarRepository<SysMenu> sysMenuRep,
                              ISysCacheService sysCacheService,
                              ISysUserRoleService sysUserRoleService,
                              ISysRoleMenuService sysRoleMenuService)
        {
            _sysMenuRep = sysMenuRep;
            _sysCacheService = sysCacheService;
            _sysUserRoleService = sysUserRoleService;
            _sysRoleMenuService = sysRoleMenuService;
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddMenu(AddMenuParam param)
        {
            var isExist = await _sysMenuRep.AnyAsync(u => u.Code == param.Code); // u.Name == input.Name
            if (isExist)
                throw Oops.Oh(ErrorCode.E4000);
            if (string.IsNullOrEmpty(param.Component))
            {
                param.Component = "LAYOUT";
            }
            // 校验参数
            CheckMenuParam(param);

            var menu = param.Adapt<SysMenu>();
            menu.Pids = await CreateNewPids(param.Pid);
            menu.Status = (int)ValidityStatus.ENABLE;
            await _sysMenuRep.InsertAsync(menu);

            // 清除缓存

            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_MENU);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_ALLPERMISSION);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteMenu(DeleteMenuParam param)
        {
            var menuInfo = await _sysMenuRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (menuInfo.SysFlag == YesOrNo.Yes)
                throw Oops.Oh(ErrorCode.E4007);

            var childIdList = await _sysMenuRep.Where(u => u.Pids.Contains(param.Id.ToString()))
                                                               .Select(u => u.Id).ToListAsync();
            childIdList.Add(param.Id);
            try
            {
                _sysMenuRep.Ado.BeginTran();
                await _sysMenuRep.DeleteAsync(u => childIdList.Contains(u.Id));
                // 级联删除该菜单及子菜单对应的角色-菜单表信息
                await _sysRoleMenuService.DeleteRoleMenuListByMenuIdList(childIdList);

                // 清除缓存
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_MENU);
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_ALLPERMISSION);
                _sysMenuRep.Ado.CommitTran();
            }
            catch (System.Exception)
            {
                _sysMenuRep.Ado.RollbackTran();
                throw;
            }
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<string>> GetAllPermission()
        {

            var permissions = await _sysCacheService.GetAllPermissionAsync(); // 先从缓存里面读取
            if (permissions == null || permissions.Count < 1)
            {
                permissions = await _sysMenuRep.Where(u => u.Type == MenuType.BTN)
                                           .Where(u => u.Status == (int)ValidityStatus.ENABLE)
                                           .Select(u => u.Permission).ToListAsync();
                await _sysCacheService.SetAllPermissionAsync(permissions); // 缓存结果
            }

            return permissions;
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<MenuTreeNode>> GetLoginMenusAntDesign(long userId)
        {
            var antDesignTreeNodes = await _sysCacheService.GetMenuAsync(userId); // 先从缓存里面读取
            if (antDesignTreeNodes == null || antDesignTreeNodes.Count < 1)
            {
                var sysMenuList = new List<SysMenu>();
                // 管理员则展示所有系统菜单
                if (AppUser.IsSuperAdmin)
                {
                    sysMenuList = await _sysMenuRep
                                                   .Where(u => u.Status == ValidityStatus.ENABLE)
                                                   .Where(u => u.Type != MenuType.BTN)
                                                   .OrderBy(u => u.Sort).ToListAsync();
                }
                else
                {
                    // 非管理员则获取自己角色所拥有的菜单集合
                    var roleIdList = await _sysUserRoleService.GetUserRoleIdListAsync(userId);
                    var menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
                    sysMenuList = await _sysMenuRep
                                                   .Where(u => menuIdList.Contains(u.Id))
                                                   .Where(u => u.Status == ValidityStatus.ENABLE)
                                                   .Where(u => u.Type != MenuType.BTN)
                                                   .OrderBy(u => u.Sort).ToListAsync();
                }
                // 转换成登录菜单
                antDesignTreeNodes = sysMenuList.Select(u => new MenuTreeNode
                {
                    Id= u.Id,
                    Pid= u.Pid,
                    Name = u.Code,
                    Icon = u.Icon,
                    Redirect = u.OpenType == MenuOpenType.OUTER ? u.Link : u.Redirect,
                    Path = u.OpenType == MenuOpenType.OUTER ? u.Link : u.Path,
                    Component = u.Component,
                    Disabled = u.Status!=ValidityStatus.ENABLE,
                    Meta = new Meta
                    {
                        Title = u.Name,
                        Icon = u.Icon,
                        Link = u.Link,
                        HideMenu = u.Visible != YesOrNo.Yes,
                        Target = u.OpenType == MenuOpenType.OUTER ? "_blank" : ""
                    }
                }).ToList();
                await _sysCacheService.SetMenuAsync(userId, antDesignTreeNodes); // 缓存结果
            }
            return antDesignTreeNodes;
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<string>> GetLoginPermissionList(long userId)
        {
            var permissions = await _sysCacheService.GetPermissionAsync(userId); // 先从缓存里面读取
            if (permissions == null || permissions.Count < 1)
            {
                if (!AppUser.IsSuperAdmin)
                {
                    var roleIdList = await _sysUserRoleService.GetUserRoleIdListAsync(userId);
                    var menuIdList = await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
                    permissions = await _sysMenuRep.Where(u => menuIdList.Contains(u.Id))
                                                               .Where(u => u.Type == MenuType.BTN)
                                                               .Where(u => u.Status == ValidityStatus.ENABLE)
                                                               .Select(u => u.Permission).ToListAsync();
                }
                else
                {
                    permissions = await _sysMenuRep.Where(u => u.Type == MenuType.BTN)
                                                               .Where(u => u.Status == ValidityStatus.ENABLE)
                                                               .Select(u => u.Permission).ToListAsync();
                }
                await _sysCacheService.SetPermissionAsync(userId, permissions); // 缓存结果
            }
            return permissions;
        }

        /// <inheritdoc/>
        [HttpPost("detail")]
        public async Task<dynamic> GetMenu(QueryMenuParam param)
        {
            return await _sysMenuRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<List<MenuView>> GetMenuList([FromQuery] MenuParam param)
        {
            var menus = await _sysMenuRep.AsQueryable()
                 .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                 .WhereIF(param.Status > -1, u => u.Status == (ValidityStatus)param.Status)
                 .OrderBy(u => u.Sort)
                 .Select<MenuView>()
                 .ToListAsync();
            return new TreeBuildUtil<MenuView>().DoTreeBuild(menus);

        }

        /// <inheritdoc/>
        [HttpGet("tree")]
        public async Task<List<MenuTreeView>> GetMenuTree([FromQuery] MenuParam param)
        {
            var menus = await _sysMenuRep.AsQueryable()
                                         .Where(u => u.Status == ValidityStatus.ENABLE)
                                         .Where(u => u.Type == MenuType.DIR || u.Type == MenuType.MENU)
                                         .OrderBy(u => u.Sort)
                                         .Select(u => new MenuTreeView
                                         {
                                             Id = u.Id,
                                             ParentId = u.Pid,
                                             Value = u.Id.ToString(),
                                             Title = u.Name,
                                             Weight = u.Sort,
                                             Icon = u.Icon
                                         }).ToListAsync();
            return new TreeBuildUtil<MenuTreeView>().DoTreeBuild(menus);
        }

        /// <inheritdoc/>
        [HttpGet("userMenu")]
        public async Task<List<MenuTreeNode>> GetUserMenu()
        {
            var antMenus = await GetLoginMenusAntDesign(AppUser.UserId);
            var menuTree = new TreeBuildUtil<MenuTreeNode>().DoTreeBuild(antMenus);
            return menuTree;
        }
        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateMenu(UpdateMenuParam param)
        {
            // Pid和Id不能一致，一致会导致无限递归
            if (param.Id == param.Pid)
                throw Oops.Oh(ErrorCode.E4006);

            var menuInfo = await _sysMenuRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (menuInfo.SysFlag == YesOrNo.Yes && (param.Status != (int)ValidityStatus.ENABLE || param.Visible != (int)YesOrNo.Yes))
                throw Oops.Oh(ErrorCode.E4008);
            if (string.IsNullOrEmpty(param.Component))
            {
                param.Component = menuInfo.Component;
            }
            var isExist = await _sysMenuRep.AnyAsync(u => u.Code == param.Code && u.Id != param.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.E4000);

            // 校验参数
            CheckMenuParam(param);

            var menuList = new List<SysMenu>();

            // 如果是编辑，父id不能为自己的子节点
            var childIdList = await _sysMenuRep.Where(u => u.Pids.Contains(param.Id.ToString()))
                                                                .Select(u => u.Id).ToListAsync();
            if (childIdList.Contains(param.Pid))
                throw Oops.Oh(ErrorCode.E4006);

            var oldMenu = await _sysMenuRep.FirstOrDefaultAsync(u => u.Id == param.Id);

            // 生成新的pids
            var newPids = await CreateNewPids(param.Pid);

            // 是否更新子应用的标识
            var updateSubAppsFlag = false;
            // 是否更新子节点的pids的标识
            var updateSubPidsFlag = false;

            // 父节点有变化
            if (param.Pid != oldMenu.Pid)
                updateSubPidsFlag = true;

            // 开始更新所有子节点的配置
            if (updateSubAppsFlag || updateSubPidsFlag)
            {
                // 查找所有叶子节点，包含子节点的子节点
                menuList = await _sysMenuRep.Where(u => u.Pids.Contains(oldMenu.Id.ToString())).ToListAsync();
                // 更新所有子节点的应用为当前菜单的应用
                if (menuList.Count > 0)
                {
                    // 更新所有子节点的pids
                    if (updateSubPidsFlag)
                    {
                        menuList.ForEach(u =>
                        {
                            // 子节点pids组成 = 当前菜单新pids + 当前菜单id + 子节点自己的pids后缀
                            var oldParentCodesPrefix = oldMenu.Pids + "[" + oldMenu.Id + "],";
                            var oldParentCodesSuffix = u.Pids.Substring(oldParentCodesPrefix.Length);
                            var menuParentCodes = newPids + "[" + oldMenu.Id + "]," + oldParentCodesSuffix;
                            u.Pids = menuParentCodes;
                        });
                    }
                }
            }

            // 更新当前菜单
            oldMenu = param.Adapt(oldMenu);
            oldMenu.Pids = newPids;

            menuList.Add(oldMenu);

            await _sysMenuRep.AsUpdateable(menuList).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();

            // 清除缓存
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_MENU);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_ALLPERMISSION);
        }

        /// <summary>
        /// 增加和编辑时检查参数
        /// </summary>
        /// <param name="param"></param>
        private static void CheckMenuParam(MenuParam param)
        {
            var type = param.Type;
            var router = param.Path;
            var permission = param.Permission;
            var openType = param.OpenType;

            if (type.Equals(MenuType.DIR))
            {
                if (string.IsNullOrEmpty(router))
                    throw Oops.Oh(ErrorCode.E4001);
            }
            else if (type.Equals(MenuType.MENU))
            {
                if (string.IsNullOrEmpty(router))
                    throw Oops.Oh(ErrorCode.E4001);
            }
            else if (type.Equals(MenuType.BTN))
            {
                if (string.IsNullOrEmpty(permission))
                    throw Oops.Oh(ErrorCode.E4003);
                if (!permission.Contains(":"))
                    throw Oops.Oh(ErrorCode.E4004);
            }
        }

        /// <summary>
        /// 创建Pids格式 
        /// 如果pid是0顶级节点，pids就是 [0];
        /// 如果pid不是顶级节点，pids就是 pid菜单的 pids + [pid] + ,
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private async Task<string> CreateNewPids(long pid)
        {
            if (pid == 0)
            {
                return $"[0],";
            }
            else
            {
                var pmenu = await _sysMenuRep.FirstOrDefaultAsync(u => u.Id == pid);
                return pmenu.Pids + "[" + pid + "],";
            }
        }
    }
}
