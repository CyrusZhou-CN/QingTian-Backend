using Furion;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using QingTian.Core.Entity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        private readonly IEventPublisher _eventPublisher;

        public LogExceptionHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var userContext = App.User;
            _eventPublisher.PublishAsync(new ChannelEventSource("Create:ExLog",
                new SysLogEx
                {
                    Account = userContext?.FindFirstValue(ConstClaim.Account),
                    Name = userContext?.FindFirstValue(ConstClaim.Name),
                    ClassName = context.Exception.TargetSite.DeclaringType?.FullName,
                    MethodName = context.Exception.TargetSite.Name,
                    ExceptionName = context.Exception.Message,
                    ExceptionMsg = context.Exception.Message,
                    ExceptionSource = context.Exception.Source,
                    StackTrace = context.Exception.StackTrace,
                    ParamsObj = context.Exception.TargetSite.GetParameters().ToString(),
                    ExceptionTime = DateTime.Now
                }));
            Log.Error(context.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
