using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 规范化RESTful风格返回值
    /// </summary>
    [SuppressSniffer, UnifyModel(typeof(QtRestfulResult<>))]
    public class QtRestfulResultProvider : IUnifyResultProvider
    {
        public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
        {
            // 解析异常信息
            var exceptionMetadata = UnifyContext.GetExceptionMetadata(context);

            return new JsonResult(new QtRestfulResult<object>
            {
                Code = exceptionMetadata.StatusCode,
                Success = false,
                Result = null,
                Message = exceptionMetadata.Errors,
                Extras = UnifyContext.Take(),
                Timestamp = DateTime.Now.Millisecond,
            });
        }

        public async Task OnResponseStatusCodes(HttpContext context, int statusCode, UnifyResultSettingsOptions unifyResultSettings = null)
        {
            Console.WriteLine("OnResponseStatusCodes");
            // 设置响应状态码
            UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

            if (Enum.IsDefined(typeof(HttpStatusCodeDes), (HttpStatusCodeDes)statusCode))
            {
                await context.Response.WriteAsJsonAsync(new QtRestfulResult<object>
                {
                    Code = statusCode,
                    Success = false,
                    Result = null,
                    Message = EnumUtil.GetDescription((HttpStatusCodeDes)statusCode),
                    Extras = UnifyContext.Take(),
                    Timestamp = DateTime.Now.Millisecond
                });
            }
        }

        public IActionResult OnSucceeded(ActionExecutedContext context, object data)
        {
            switch (context.Result)
            {
                // 处理内容结果
                case ContentResult contentResult:
                    data = contentResult.Content;
                    break;
                // 处理对象结果
                case ObjectResult objectResult:
                    data = objectResult.Value;
                    break;
                case EmptyResult:
                    data = null;
                    break;
                default:
                    return null;
            }
            return new JsonResult(new QtRestfulResult<object>
            {
                Code = context.Result is EmptyResult ? StatusCodes.Status204NoContent : StatusCodes.Status200OK,  // 处理没有返回值情况 204
                Success = true,
                Result = data,
                Message = "请求成功",
                Extras = UnifyContext.Take(),
                Timestamp = DateTime.Now.Millisecond
            });
        }

        public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
        {
            return new JsonResult(new QtRestfulResult<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Result = null,
                Message = metadata.ValidationResult,
                Extras = UnifyContext.Take(),
                Timestamp = DateTime.Now.Millisecond
            });
        }
    }
}
