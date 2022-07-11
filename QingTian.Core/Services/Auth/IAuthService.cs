using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 登录授权相关服务
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        Task<LoginInfoResult> GetUserinfo();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <remarks>默认用户名/密码：superAdmin/123456</remarks>
        /// <returns></returns>
        Task<LoginResult> Login([Required] LoginParam param);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetPermCode();
    }
}
