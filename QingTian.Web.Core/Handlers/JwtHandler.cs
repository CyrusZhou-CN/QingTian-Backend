using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using QingTian.Core;
using QingTian.Core.Services;
using System.Threading.Tasks;

namespace QingTian.Web.Core
{
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新收取逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
            {
                await AuthorizeHandleAsync(context);
            }
            else context.Fail();    // 授权失败
        }

        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 此处已经自动验证 Jwt token的有效性了，无需手动验证
            // 检查权限，如果方法是异步的就不用 Task.FromResult 包裹，直接使用 async/await 即可
            return await CheckAuthorzieAsync(httpContext);
        }

        private static async Task<bool> CheckAuthorzieAsync(DefaultHttpContext httpContext)
        {
            // 管理员跳过判断
            if (AppUser.IsSuperAdmin) return true;
            // 路由名称
            var routeName = httpContext.Request.Path.Value.Substring(1).Replace("/", ":");

            var allPermission = await App.GetService<ISysMenuService>().GetAllPermission();

            if (!allPermission.Contains(routeName))
            {
                return true;
            }

            // 获取用户权限集合（按钮或API接口）
            var permissionList = await App.GetService<ISysMenuService>().GetLoginPermissionList(AppUser.UserId);

            // 检查授权
            return permissionList.Contains(routeName);
        }
    }
}
