﻿using Furion;
using Furion.EventBus;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UAParser;

namespace QingTian.Core
{
    /// <summary>
    /// 请求日志拦截
    /// </summary>
    public class RequestActionFilter : IAsyncActionFilter
    {
        private readonly IEventPublisher _eventPublisher; 
        private readonly SqlSugarRepository<SysUser> _sysUserRep;

        public RequestActionFilter(SqlSugarRepository<SysUser> sysUserRep,
            IEventPublisher eventPublisher)
        {
            _sysUserRep = sysUserRep;
            _eventPublisher = eventPublisher;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;

            var sw = new Stopwatch();
            sw.Start();
            var actionContext = await next();
            sw.Stop();

            // 判断是否请求成功（没有异常就是请求成功）
            var isRequestSucceed = actionContext.Exception == null;
            var headers = httpRequest.Headers;
            var clientInfo = headers.ContainsKey("User-Agent") ? Parser.GetDefault().Parse(headers["User-Agent"]) : null;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var isWriteLog = false;
            var ip = HttpNewUtil.Ip;
            //判断是否需有禁用操作日志属性
            foreach (var metadata in actionDescriptor.EndpointMetadata)
            {
                if (metadata.GetType() == typeof(OpLogAttribute))
                {
                    isWriteLog = true;
                    break;
                }
            }
            //请求异常时记录日志
            if (!isRequestSucceed || App.GetOptions<SystemSettingsOptions>().IsGlobalRequestLog)
            {
                isWriteLog = true;
            }
            if (isWriteLog)
            {
                var opLog = new SysLogOp
                {
                    Name = httpContext.User?.FindFirstValue(ConstClaim.Name),
                    Success = isRequestSucceed ? YesOrNo.Yes : YesOrNo.No,
                    Ip = ip,
                    Location = httpRequest.GetRequestUrlAddress(),
                    Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                    Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
                    Url = httpRequest.Path,
                    Message = actionContext.Result?.GetType() == typeof(JsonResult) ? ((QingTian.Core.QtRestfulResult<object>)((JsonResult)actionContext.Result).Value).Message.ToString() : "",
                    ClassName = context.Controller.ToString(),
                    MethodName = actionDescriptor?.ActionName,
                    ReqMethod = httpRequest.Method,
                    Param = JSON.Serialize(context.ActionArguments.Count < 1 ? null : context.ActionArguments),
                    Result = actionContext.Result?.GetType() == typeof(JsonResult) ? JSON.Serialize(actionContext.Result) : null,
                    ElapsedTime = sw.ElapsedMilliseconds,
                    OpTime = DateTime.Now,
                    Account = httpContext.User?.FindFirstValue(ConstClaim.Account)
                };
                if (string.IsNullOrWhiteSpace(opLog.Account) && opLog.MethodName == "login")
                {
                    opLog.Account = context.ActionArguments.ContainsKey("param") ? ((QingTian.Core.Services.LoginParam)context.ActionArguments["param"]).Account : "unknown";
                    opLog.Name = _sysUserRep.FirstOrDefault(m=>m.Account==opLog.Account)?.RealName;
                }
                await _eventPublisher.PublishAsync(new ChannelEventSource("Create:OpLog", opLog));
            }
        }
    }
}
