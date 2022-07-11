using Furion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QingTian.Core.Cache;
using QingTian.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    public static class SqlSugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            #region 配置sqlsuagr
            List<ConnectionConfig> connectConfigList = new List<ConnectionConfig>();
            //数据库序号从0开始,默认数据库为0
            var config = App.GetOptions<ConnectionStringsOptions>();
            //默认数据库
            var defaultDbConfig = new ConnectionConfig
            {
                ConnectionString = config.DefaultDbString,
                DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), config.DefaultDbType)),
                IsAutoCloseConnection = true,
                ConfigId = config.DefaultDbNumber,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true//自动清理缓存
                }
            };
            // 初始化系统数据库
            InitDB(defaultDbConfig);

            connectConfigList.Add(defaultDbConfig);
            if (config.DbConfigs == null)
                config.DbConfigs = new List<DbConfig>();
            //业务数据库集合
            foreach (var item in config.DbConfigs)
            {
                //防止数据库重复，导致的事务异常
                if (connectConfigList.Any(a => a.ConfigId == (dynamic)item.DbNumber || a.ConnectionString == item.DbString))
                {
                    continue;
                }
                connectConfigList.Add(new ConnectionConfig
                {
                    ConnectionString = item.DbString,
                    DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), item.DbType)),
                    IsAutoCloseConnection = true,
                    ConfigId = item.DbNumber,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true//自动清理缓存
                    },
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            Console.WriteLine(sql);
                            Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                        }
                    }
                });
            }

            List<Type> types = App.EffectiveTypes.Where(a => !a.IsAbstract && a.IsClass && a.GetCustomAttributes(typeof(SugarTable), true)?.FirstOrDefault() != null).ToList();
            //sugar action
            Action<ISqlSugarClient> configure = db =>
            {
                connectConfigList.ForEach(config =>
                {
                    string temp = config.ConfigId;
                    var _db = db.AsTenant().GetConnection(temp);
                    _db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        DataInfoCacheService = new SqlSugarCache()//配置我们创建的缓存类
                    };
                    //执行超时时间
                    _db.Ado.CommandTimeOut = 30;
                    _db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        if (sql.StartsWith("SELECT"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        if (sql.StartsWith("UPDATE") || sql.StartsWith("INSERT"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        if (sql.StartsWith("DELETE"))
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.WriteLine(sql + "\r\n\r\n" + SqlProfiler.ParameterFormat(sql, pars));
                        App.PrintToMiniProfiler("SqlSugar", "Info", SqlProfiler.ParameterFormat(sql, pars));
                    };
                    foreach (var entityType in types)
                    {
                        // 配置加删除全局过滤器
                        if (!entityType.GetProperty(ConstCache.DELETE_FIELD).IsEmpty())
                        { //判断实体类中包含IsDeleted属性
                          //构建动态Lambda
                            var lambda = DynamicExpressionParser.ParseLambda
                            (new[] { Expression.Parameter(entityType, "it") },
                             typeof(bool), $"{nameof(DbEntityBase.IsDeleted)} ==  @0",
                              false);
                            _db.QueryFilter.Add(new TableFilterItem<object>(entityType, lambda)
                            {
                                IsJoinQuery = true
                            }); //将Lambda传入过滤器
                        }
                    }
                });
            };
            services.AddSqlSugar(connectConfigList, configure);
            #endregion
        }
        /// <summary>
        /// 初始化系统数据库
        /// </summary>
        /// <param name="defaultDbConfig"></param>
        private static void InitDB(ConnectionConfig defaultDbConfig)
        {
            SqlSugarClient db = new SqlSugarClient(defaultDbConfig);
            var isCreate = db.DbMaintenance.CreateDatabase();

            Type[] types = Assembly.Load("QingTian.Core").GetTypes()
            .Where(it => it.FullName.Contains("QingTian.Core.Entity.Sys"))//命名空间过滤，当然你也可以写其他条件过滤
            .ToArray();
            db.CodeFirst.SetStringDefaultLength(200).InitTables(types);//根据types创建表

            Type[] app_types = Assembly.Load("QingTian.Application").GetTypes()
            .Where(it => it.FullName.Contains("QingTian.Application.Entity"))//命名空间过滤，当然你也可以写其他条件过滤
            .ToArray();
            db.CodeFirst.SetStringDefaultLength(200).InitTables(app_types);//根据types创建表

            #region 系统设置
            // 检查 超级管理员
            var superAdmin = db.Queryable<SysUser>().First(m => m.Account == "superAdmin");
            if (superAdmin == null)
            {
                var s = db.Insertable(new SysUser
                {
                    Account = "superAdmin",
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    NickName = "superAdmin",
                    RealName = "超级管理员",
                    UserType = UserType.SuperAdmin
                }).CallEntityMethod(m => m.Create()).ExecuteReturnEntity();
            }
            #endregion
            #region 系统菜单

            var dashboard = db.Queryable<SysMenu>().First(m => m.Code == "dashboard");
            if (dashboard == null)
            {
                List<SysMenu> menus = new List<SysMenu>();
                var pid = SnowFlakeSingle.Instance.NextId();
                var pmenu = new SysMenu
                {
                    Id = pid,
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "仪表板",
                    Code = "dashboard",
                    Type = MenuType.DIR,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:home-outlined",
                    Path = "/dashboard",
                    Component = "LAYOUT",
                    Redirect = "/dashboard/analysis"
                };
                menus.Add(pmenu);

                menus.Add(new SysMenu
                {
                    Pid = pmenu.Id,
                    Pids = $"[0],[{pid}],",
                    Name = "分析页",
                    Code = "analysis",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:area-chart-outlined",
                    Path = "analysis",
                    Component = "/dashboard/analysis/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = pmenu.Id,
                    Pids = $"[0],[{pid}],",
                    Name = "工作台",
                    Code = "workbench",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:desktop-outlined",
                    Path = "workbench",
                    Component = "/dashboard/workbench/index",
                });
                db.Insertable(menus).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();

            }
            // 系统管理菜单
            var sys_manager = db.Queryable<SysMenu>().First(m => m.Code == "sys_manager");
            if (sys_manager == null)
            {
                var pid = SnowFlakeSingle.Instance.NextId();
                List<SysMenu> menus = new List<SysMenu>();
                var pmenu = new SysMenu
                {
                    Id = pid,
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "系统管理",
                    Code = "sys_manager",
                    Type = MenuType.DIR,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:setting-outlined",
                    Path = "/system",
                    Component = "LAYOUT",
                    Redirect = "/sys/account/index"
                };
                menus.Add(pmenu);

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "用户管理",
                    Code = "account",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:team-outlined",
                    Path = "account",
                    Component = "/sys/account/index",
                });

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "在线用户",
                    Code = "onlineUser",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:smile-outlined",
                    Path = "onlineUser",
                    Component = "/sys/onlineUser/index",
                });

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "组织管理",
                    Code = "organize",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:cluster-outlined",
                    Path = "organize",
                    Component = "/sys/organize/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "职务管理",
                    Code = "position",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:audit-outlined",
                    Path = "position",
                    Component = "/sys/position/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "角色管理",
                    Code = "role",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:safety-outlined",
                    Path = "role",
                    Component = "/sys/role/index",
                });
                db.Insertable(menus).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            // 通知消息管理菜单
            var notice_management = db.Queryable<SysMenu>().First(m => m.Code == "notice_management");
            if (notice_management == null)
            {
                var pid = SnowFlakeSingle.Instance.NextId();
                List<SysMenu> menus = new List<SysMenu>();
                var pmenu = new SysMenu
                {
                    Id = pid,
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "通知消息",
                    Code = "notice_management",
                    Type = MenuType.DIR,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:notification-outlined",
                    Path = "/notice",
                    Component = "LAYOUT",
                    Redirect = "/sys/notice/index"
                };
                menus.Add(pmenu);
                var exid = SnowFlakeSingle.Instance.NextId();
                menus.Add(new SysMenu
                {
                    Id = exid,
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "通知管理",
                    Code = "notice",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:notification-twotone",
                    Path = "index",
                    Component = "/sys/notice/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "通知查询",
                    Code = "notice_page",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:search-outlined",
                    Permission = "SysNotice:page",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "通知详情",
                    Code = "notice_detail",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:profile-outlined",
                    Permission = "SysNotice:detail",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "通知删除",
                    Code = "notice_delete",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:delete-outlined",
                    Permission = "SysNotice:delete",
                });
                var opid = SnowFlakeSingle.Instance.NextId();
                menus.Add(new SysMenu
                {
                    Id = opid,
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "已收通知",
                    Code = "unread_notice",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:notification-filled",
                    Path = "unread",
                    Component = "/sys/notice/unreadNotice",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "已收通知查询",
                    Code = "unread_notice_page",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:search-outlined",
                    Permission = "unreadNotice:page",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "已收通知详情",
                    Code = "unread_notice_detail",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:profile-outlined",
                    Permission = "unreadNotice:detail",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "已收通知清空",
                    Code = "unread_notice_delete",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:delete-outlined",
                    Permission = "unreadNotice:delete",
                });
                db.Insertable(menus).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            // 日志管理菜单
            var log_management = db.Queryable<SysMenu>().First(m => m.Code == "log_management");
            if (log_management == null)
            {
                var pid = SnowFlakeSingle.Instance.NextId();
                List<SysMenu> menus = new List<SysMenu>();
                var pmenu = new SysMenu
                {
                    Id = pid,
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "日志管理",
                    Code = "log_management",
                    Type = MenuType.DIR,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:alert-outlined",
                    Path = "/log",
                    Component = "LAYOUT",
                    Redirect = "/log/sysLogOp/index"
                };
                menus.Add(pmenu);
                var exid = SnowFlakeSingle.Instance.NextId();
                menus.Add(new SysMenu
                {
                    Id = exid,
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "异常日志",
                    Code = "syslogex",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:bug-outlined",
                    Path = "logex",
                    Component = "/log/sysLogEx/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "异常日志查询",
                    Code = "syslogex_page",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:search-outlined",
                    Permission = "SysLogEx:page",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "异常日志详情",
                    Code = "syslogex_detail",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:profile-outlined",
                    Permission = "SysLogEx:detail",
                });
                menus.Add(new SysMenu
                {
                    Pid = exid,
                    Pids = $"[0],[{pid}],[{exid}],",
                    Name = "异常日志清空",
                    Code = "syslogex_delete",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:delete-outlined",
                    Permission = "SysLogEx:delete",
                });
                var opid = SnowFlakeSingle.Instance.NextId();
                menus.Add(new SysMenu
                {
                    Id = opid,
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "操作日志",
                    Code = "syslogop",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:database-outlined",
                    Path = "logop",
                    Component = "/log/sysLogOp/index",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "操作日志查询",
                    Code = "syslogop_page",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:search-outlined",
                    Permission = "SysLogOp:page",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "操作日志详情",
                    Code = "syslogop_detail",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:profile-outlined",
                    Permission = "SysLogOp:detail",
                });
                menus.Add(new SysMenu
                {
                    Pid = opid,
                    Pids = $"[0],[{pid}],[{opid}],",
                    Name = "操作日志清空",
                    Code = "syslogop_delete",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:delete-outlined",
                    Permission = "SysLogOp:delete",
                });

                var visid = SnowFlakeSingle.Instance.NextId();
                menus.Add(new SysMenu
                {
                    Id = visid,
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "访问日志",
                    Code = "syslogvis",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:menu-outlined",
                    Path = "logvis",
                    Component = "/log/sysLogVis/index",
                }); menus.Add(new SysMenu
                {
                    Pid = visid,
                    Pids = $"[0],[{pid}],[{visid}],",
                    Name = "操作日志查询",
                    Code = "syslogvis_page",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:search-outlined",
                    Permission = "SysLogVis:page",
                });
                menus.Add(new SysMenu
                {
                    Pid = visid,
                    Pids = $"[0],[{pid}],[{visid}],",
                    Name = "操作日志详情",
                    Code = "syslogvis_detail",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:profile-outlined",
                    Permission = "SysLogVis:detail",
                });
                menus.Add(new SysMenu
                {
                    Pid = visid,
                    Pids = $"[0],[{pid}],[{visid}],",
                    Name = "操作日志清空",
                    Code = "syslogvis_delete",
                    Type = MenuType.BTN,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:delete-outlined",
                    Permission = "SysLogVis:delete",
                });
                db.Insertable(menus).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            // 开发管理菜单
            var dev_management = db.Queryable<SysMenu>().First(m => m.Code == "dev_management");
            if (dev_management == null)
            {
                var pid = SnowFlakeSingle.Instance.NextId();
                List<SysMenu> menus = new List<SysMenu>();
                var pmenu = new SysMenu
                {
                    Id = pid,
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "开发管理",
                    Code = "dev_management",
                    Type = MenuType.DIR,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:tool-outlined",
                    Path = "/develop",
                    Component = "LAYOUT",
                    Redirect = "/gen/database/index"
                };
                menus.Add(pmenu);

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "代码生成",
                    Code = "code",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:thunderbolt-outlined",
                    Path = "codeGenerate",
                    Component = "/gen/codeGenerate/index",
                });

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "数据库管理",
                    Code = "database",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:database-outlined",
                    Path = "database",
                    Component = "/gen/database/index",
                });

                menus.Add(new SysMenu
                {
                    Pid = pid,
                    Pids = $"[0],[{pid}],",
                    Name = "菜单管理",
                    Code = "menu",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:menu-outlined",
                    Path = "menu",
                    Component = "/sys/menu/index",
                });
                db.Insertable(menus).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            var about = db.Queryable<SysMenu>().First(m => m.Code == "about");
            if (about == null)
            {
                db.Insertable(new SysMenu
                {
                    Pid = 0,
                    Pids = $"[0],",
                    Name = "关于",
                    Code = "about",
                    Type = MenuType.MENU,
                    SysFlag = YesOrNo.Yes,
                    Icon = "ant-design:info-circle-outlined",
                    Path = "about",
                    Component = "/sys/about/index",
                }).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            #endregion
            #region 默认字典
            var dictType = db.Queryable<SysDictType>().First(m => m.Code == "gender");
            var dictData = new List<SysDictData>();
            if (dictType == null)
            {
                dictData = new List<SysDictData>();
                dictType = db.Insertable(new SysDictType
                {
                    Code = "gender",
                    Name = "性别"
                }).CallEntityMethod(m => m.Create()).ExecuteReturnEntity();
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "1",
                    Value = "男",
                    Remark = "男性"
                });
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "2",
                    Value = "女",
                    Remark = "女性"
                });
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "3",
                    Value = "保密",
                    Remark = "保密性别"
                });
                db.Insertable(dictData).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            dictType = db.Queryable<SysDictType>().First(m => m.Code == "yes_or_no");
            if (dictType == null)
            {
                dictData = new List<SysDictData>();
                dictType = db.Insertable(new SysDictType
                {
                    Code = "yes_or_no",
                    Name = "是否"
                }).CallEntityMethod(m => m.Create()).ExecuteReturnEntity();
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "1",
                    Value = "是",
                    Remark = "是"
                });
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "0",
                    Value = "否",
                    Remark = "否"
                });
                db.Insertable(dictData).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            dictType = db.Queryable<SysDictType>().First(m => m.Code == "validity_status");
            if (dictType == null)
            {
                dictData = new List<SysDictData>();
                dictType = db.Insertable(new SysDictType
                {
                    Code = "validity_status",
                    Name = "状态"
                }).CallEntityMethod(m => m.Create()).ExecuteReturnEntity();
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "0",
                    Value = "正常",
                    Remark = "正常"
                });
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "1",
                    Value = "停用",
                    Remark = "停用"
                });
                dictData.Add(new SysDictData
                {
                    TypeId = dictType.Id,
                    Code = "2",
                    Value = "删除",
                    Remark = "删除"
                });
                db.Insertable(dictData).CallEntityMethod(m => m.Create()).ExecuteCommandAsync();
            }
            #endregion

        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, List<ConnectionConfig> configs, Action<ISqlSugarClient>? buildAction = default)
        {
            // 注册 SqlSugar 客户端
            services.AddScoped<ISqlSugarClient>(u =>
            {
                var db = new SqlSugarClient(configs);
                buildAction?.Invoke(db);
                return db;
            });

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(SqlSugarRepository<>));
            services.AddScoped(typeof(SqlSugarRepository));

            return services;
        }
    }
}
