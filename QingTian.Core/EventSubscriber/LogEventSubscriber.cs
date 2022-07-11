using Furion.EventBus;
using Microsoft.Extensions.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// Log 日志订阅
    /// </summary>
    public class LogEventSubscriber : IEventSubscriber
    {
        public LogEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        [EventSubscribe("Create:OpLog")]
        public async Task CreateOpLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogOp>>();
            var log = (SysLogOp)context.Source.Payload;            
            await _repository.InsertAsync(log);
        }

        [EventSubscribe("Create:ExLog")]
        public async Task CreateExLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
            var log = (SysLogEx)context.Source.Payload;
            await _repository.InsertAsync(log);
        }

        [EventSubscribe("Create:VisLog")]
        public async Task CreateVisLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogVis>>();
            var log = (SysLogVis)context.Source.Payload;
            await _repository.InsertAsync(log);
        }

        [EventSubscribe("Update:UserLoginInfo")]
        public async Task UpdateUserLoginInfo(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysUser>>();
            var log = (SysUser)context.Source.Payload;
            await _repository.AsUpdateable(log).UpdateColumns(m => new { m.LastLoginTime, m.LastLoginIp })
                .ExecuteCommandAsync();
        }
    }
}
