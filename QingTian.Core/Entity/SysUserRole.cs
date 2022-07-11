using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [SugarTable("sys_user_role")]
    [Description("用户角色表")]
    public class SysUserRole : IEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long SysRoleId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long SysUserId { get; set; }

        public void Create()
        {
        }

        public void Modify()
        {
        }
    }
}
