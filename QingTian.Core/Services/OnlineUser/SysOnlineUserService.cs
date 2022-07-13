using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.OnlineUser
{
    [Route("SysOnlineUser"), ApiDescriptionSettings(Name = "OnlineUser", Order = 995)]
    public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
    {
        private readonly ISysCacheService _sysCacheService;
        private readonly SqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;  // 在线用户表
        private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

        public SysOnlineUserService(ISysCacheService sysCacheService
            , SqlSugarRepository<SysOnlineUser> sysOnlineUerRep
            , IHubContext<ChatHub, IChatClient> chatHubContext)
        {
            _sysCacheService = sysCacheService;
            _sysOnlineUerRep = sysOnlineUerRep;
            _chatHubContext = chatHubContext;
        }

        [HttpPost("forceExist")]
        public async Task ForceExist(SysOnlineUser user)
        {
            await _chatHubContext.Clients.Client(user.ConnectionId).ForceExist("666666");
            await _sysOnlineUerRep.DeleteAsync(user);
        }

        [HttpGet("page")]
        public async Task<dynamic> Page([FromQuery] PageParamBase param)
        {
            var list = await _sysOnlineUerRep.AsQueryable().ToPagedListAsync(param.Page, param.PageSize);
            return list.QtPagedResult();
        }

        [NonAction]
        public async Task PushNotice(HeaderNoticeReceiveView notice, List<long> userIds)
        {
            var userList = _sysOnlineUerRep.Where(m => userIds.Contains(m.UserId)).ToList();
            if (userList.Any())
            {
                await _chatHubContext.Clients
                    .Clients(userList.Select(m => m.ConnectionId))
                    .AppendNotice(notice);
            }
        }
    }
}
