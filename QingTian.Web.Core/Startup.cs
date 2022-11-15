using Furion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingTian.Core;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Serilog;

namespace QingTian.Web.Core
{
    [AppStartup(800)]
    public sealed class FurWebCoreStartup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<ConnectionStringsOptions>();
            services.AddConfigurableOptions<JWTSettingsOptions>();
            services.AddConfigurableOptions<CacheOptions>();
            services.AddConfigurableOptions<SystemSettingsOptions>();

            //单位是字节（byte） 1kb=1024byte，默认是30M
            long maxRequestBodySize = App.GetOptions<SystemSettingsOptions>().MaxRequestBodySize;
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = maxRequestBodySize;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = maxRequestBodySize;
            });

            services.AddSqlsugarSetup();

            services.AddCorsAccessor();
            //services.AddControllers().AddInject();
            // 启用全局授权，这样每个接口都必须授权才能访问，无需贴 `[Authorize]` 特性，推荐！！！！！！！！！❤️
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

            services.AddControllersWithViews()
                    .AddMvcFilter<RequestActionFilter>()
                    .AddInjectWithUnifyResult<QtRestfulResultProvider>()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DefaultBufferSize = 10_0000;//返回较大数据数据序列化时会截断，原因：默认缓冲区大小（以字节为单位）为16384。
                        options.JsonSerializerOptions.Converters.AddDateTimeTypeConverters("yyyy-MM-dd HH:mm:ss");
                        options.JsonSerializerOptions.Converters.Add(new LongJsonConverter()); // 配置过长的整数类型返回前端会丢失精度的问题
                        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // 忽略循环引用 仅.NET 6支持
                    });

            services.AddViewEngine();
            // 添加即时通讯
            services.AddSignalR();

            services.AddRemoteRequest();
            // 注册EventBus服务
            services.AddEventBus(builder =>
            {
                // 注册 Log 日志订阅者
                builder.AddSubscriber<LogEventSubscriber>();
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                //env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                env.WebRootPath = Directory.GetCurrentDirectory();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            // 添加状态码拦截中间件
            app.UseUnifyResultStatusCodes();

            app.UseHttpsRedirection(); // 强制https

            app.UseStaticFiles();
            // Serilog请求日志中间件---必须在 UseStaticFiles 和 UseRouting 之间
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // 注册集线器
                endpoints.MapHub<ChatHub>("/hubs/chathub");
            });
        }
    }
}