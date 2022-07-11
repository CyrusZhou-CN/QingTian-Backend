using SqlSugar;
using System.ComponentModel;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [SugarTable("sys_role_menu")]
    [Description("角色菜单表")]
    public class SysRoleMenu : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long SysRoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long SysMenuId { get; set; }

        public void Create()
        {
        }

        public void Modify()
        {
        }
    }
}