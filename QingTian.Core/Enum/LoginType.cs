using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 登陆类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登陆")]
        LOGIN = 0,

        /// <summary>
        /// 登出
        /// </summary>
        [Description("登出")]
        LOGOUT = 1,

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        REGISTER = 2,

        /// <summary>
        /// 改密
        /// </summary>
        [Description("改密")]
        CHANGEPASSWORD = 3,

        /// <summary>
        /// 第三方登陆
        /// </summary>
        [Description("第三方登陆")]
        AUTHORIZEDLOGIN = 4,

        /// <summary>
        /// 权限信息
        /// </summary>
        [Description("权限信息")]
        PERMISSIONS = 5
    }
}
