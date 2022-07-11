using Furion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 当前用户状态
    /// </summary>
    public static class AppUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public static long UserId => long.Parse(App.User.FindFirst(ConstClaim.UserId)?.Value);
        /// <summary>
        /// 账号
        /// </summary>
        public static string Account => App.User.FindFirst(ConstClaim.Account)?.Value;
        /// <summary>
        /// 用户名称
        /// </summary>
        public static string UserName => App.User.FindFirst(ConstClaim.Name)?.Value;

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static bool IsSuperAdmin => App.User.FindFirst(ConstClaim.SuperAdmin)?.Value == ((int)UserType.SuperAdmin).ToString();
    }
}
