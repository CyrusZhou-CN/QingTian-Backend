using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 用户类型
    /// </summary>
    [Description("用户类型")]
    public enum UserType
    {

        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        User = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 2,
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 3,
    }
}
