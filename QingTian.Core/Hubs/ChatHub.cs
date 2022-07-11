using Furion.DataEncryption;
using Furion.InstantMessaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using QingTian.Core.Entity;
using QingTian.Core.Services;

namespace QingTian.Core
{

    /// <summary>
    /// 聊天集线器
    /// </summary>
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ISysCacheService _cache;
        private readonly SqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;  // 在线用户表


        public ChatHub(ISysCacheService cache, SqlSugarRepository<SysOnlineUser> sysOnlineUerRep)
        {
            _sysOnlineUerRep = sysOnlineUerRep;
            _cache = cache;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var claims = JWTEncryption.ReadJwtToken(token)?.Claims;
            var userId = claims.FirstOrDefault(e => e.Type == ConstClaim.UserId)?.Value;
            var account = claims.FirstOrDefault(e => e.Type == ConstClaim.Account)?.Value;
            var name = claims.FirstOrDefault(e => e.Type == ConstClaim.Name)?.Value;
            var ip = HttpNewUtil.Ip;
            if (_sysOnlineUerRep.Any(m => m.Account == account && m.LastLoginIp == ip))
            {
                await _sysOnlineUerRep.DeleteAsync(m => m.Account == account && m.LastLoginIp == ip);
            }

            var user = new SysOnlineUser()
            {
                ConnectionId = Context.ConnectionId,
                UserId = long.Parse(userId),
                LastTime = DateTime.Now,
                LastLoginIp = ip,
                Account = account,
                Name = name
            };
            await _sysOnlineUerRep.InsertAsync(user);
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (!string.IsNullOrEmpty(Context.ConnectionId))
            {
                await _sysOnlineUerRep.DeleteAsync(m => m.ConnectionId == Context.ConnectionId);
            }
        }

    }
}