using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 在线用户服务
    /// </summary>
    public interface ISysOnlineUserService
    {
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <returns></returns>
        Task<dynamic> Page(PageParamBase param);

        Task ForceExist(SysOnlineUser user);

        Task PushNotice(HeaderNoticeReceiveView notice, List<long> userIds);
    }
}
