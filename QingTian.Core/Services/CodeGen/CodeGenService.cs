using Furion;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.ViewEngine;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.CodeGen
{
    /// <inheritdoc cref="ICodeGenConfigService"/>
    [Route("CodeGenerate"), ApiDescriptionSettings(Name = "CodeGen", Order = 998)]
    public class CodeGenService : ICodeGenService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysCodeGen> _sysCodeGenRep; // 代码生成器
        private readonly ICodeGenConfigService _codeGenConfigService;
        private readonly IViewEngine _viewEngine;
        private readonly ISysCacheService _sysCacheService;

        private readonly SqlSugarRepository<SysMenu> _sysMenuRep; // 菜单表

        private readonly ICommonService _commonService;

        public CodeGenService(SqlSugarRepository<SysCodeGen> sysCodeGenRep,
                              ISysCacheService sysCacheService,
                              ICodeGenConfigService codeGenConfigService,
                              IViewEngine viewEngine,
                              ICommonService commonService,
                              SqlSugarRepository<SysMenu> sysMenuRep)
        {
            _sysCacheService = sysCacheService;
            _sysCodeGenRep = sysCodeGenRep;
            _codeGenConfigService = codeGenConfigService;
            _viewEngine = viewEngine;
            _sysMenuRep = sysMenuRep;
            _commonService = commonService;
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddCodeGen(AddCodeGenParam param)
        {
            var isExist = await _sysCodeGenRep.AnyAsync(u => u.TableName == param.TableName);
            if (isExist)
                throw Oops.Oh(ErrorCode.E1400);

            var codeGen = param.Adapt<SysCodeGen>();
            var newCodeGen = await _sysCodeGenRep.InsertReturnEntityAsync(codeGen);
            // 加入配置表中
            _codeGenConfigService.AddList(await GetColumnList(param), newCodeGen);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteCodeGen(List<DeleteCodeGenParam> param)
        {
            if (param == null || param.Count < 1) return;

            var codeGenConfigTaskList = new List<Task>();
            param.ForEach(u =>
            {
                _sysCodeGenRep.Delete(u.Id);

                // 删除配置表中
                codeGenConfigTaskList.Add(_codeGenConfigService.Delete(u.Id));
            });
            await Task.WhenAll(codeGenConfigTaskList);
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenParam param)
        {
            return await _sysCodeGenRep.SingleAsync(m => m.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateCodeGen(UpdateCodeGenParam param)
        {
            var isExist = await _sysCodeGenRep.AnyAsync(u => u.TableName == param.TableName && u.Id != param.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.E1400);
            var codeGen = await _sysCodeGenRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            codeGen = param.Adapt<UpdateCodeGenParam, SysCodeGen>(codeGen);
            await _sysCodeGenRep.UpdateAsync(codeGen);
        }

        /// <inheritdoc/>
        [HttpGet("ColumnList")]
        public async Task<List<TableColumnResult>> GetColumnListByTableParam([FromQuery] TableParam param)
        {
            if (string.IsNullOrWhiteSpace(param.TableName))
            {
                param.TableName = await GetTableByEntityName(param.EntityName);
            }
            // 获取实体类型属性
            DbTableInfo entityType = _sysCodeGenRep.Context.DbMaintenance.GetTableInfoList().FirstOrDefault(u => u.Name == param.TableName);
            if (entityType == null) return null;

            // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
            return _sysCodeGenRep.Context.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new TableColumnResult
            {
                ColumnName = u.DbColumnName,
                ColumnKey = u.IsPrimarykey,
                DataType = u.DataType.ToString(),
                NetType = CodeGenUtil.ConvertDataType(u.DataType.ToString()),
                ColumnComment = u.ColumnDescription
            }).ToList();
        }

        /// <inheritdoc/>
        [HttpGet("TableList")]
        public async Task<List<TableResult>> GetTableList()
        {
            IEnumerable<EntityInfo> entityInfos = await _commonService.GetEntityInfos(false);
            List<TableResult> result = new List<TableResult>();
            foreach (var item in entityInfos)
            {
                result.Add(new TableResult()
                {
                    EntityName = item.EntityName,
                    TableName = item.DbTableName,
                    TableComment = item.TableDescription
                });
            }
            return result;
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryCodeGenPageList([FromQuery] CodeGenPageParam param)
        {
            var tableName = !string.IsNullOrEmpty(param.TableName?.Trim());
            var codeGens = await _sysCodeGenRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(param.TableName), u => u.TableName.Contains(param.TableName.Trim()))
                                               .ToPagedListAsync(param.Page, param.PageSize);
            return codeGens.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("runLocal")]
        public async Task RunLocal(UpdateCodeGenParam param)
        {

            // 先删除该表已生成的菜单列表
            var templatePathList = GetTemplatePathList();
            var targetPathList = GetTargetPathList(param);
            var IsDict = false;
            var IsFk = false;
            for (var i = 0; i < templatePathList.Count; i++)
            {
                var tContent = File.ReadAllText(templatePathList[i]);

                var tableFieldList = await _codeGenConfigService.List(new CodeGenConfig() { CodeGenId = param.Id }); // 字段集合
                tableFieldList.ForEach(u =>
                {
                    u.EffectType = u.EffectType != "fk" ? u.EffectType.FirstCharToUpper() : u.EffectType;
                    if (!u.DictTypeCode.IsNullOrZero() && !IsDict)
                    {
                        IsDict = true;
                    }
                    if (u.EffectType == "fk" && !IsFk)
                    {
                        IsFk = true;
                    }
                });
                var queryWhetherList = tableFieldList.Where(u => u.QueryWhether == YesOrNo.Yes.ToBool()).ToList(); // 前端查询集合
                var tResult = _viewEngine.RunCompileFromCached(tContent, new
                {
                    param.AuthorName,
                    param.BusName,
                    param.NameSpace,
                    ClassName = param.TableName,
                    LowerClassName = param.TableName.FirstCharToLower(),
                    QueryWhetherList = queryWhetherList,
                    IsDict = IsDict,
                    IsFk = IsFk,
                    TableField = tableFieldList
                });

                var dirPath = new DirectoryInfo(targetPathList[i]).Parent.FullName;
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                File.WriteAllText(targetPathList[i], tResult, Encoding.UTF8);
            }

            await AddMenu(param.TableName, param.BusName, param.MenuPid);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<List<TableColumnResult>> GetColumnList(AddCodeGenParam param)
        {
            var entityType = _commonService.GetEntityInfos().Result.FirstOrDefault(m => m.EntityName == param.TableName);
            if (entityType == null)
                return null;

            return _sysCodeGenRep.Context.DbMaintenance.GetColumnInfosByTableName(entityType.DbTableName, false).Select(u => new TableColumnResult
            {
                ColumnName = u.DbColumnName,
                ColumnKey = u.IsPrimarykey,
                DataType = u.DataType.ToString(),
                ColumnComment = string.IsNullOrWhiteSpace(u.ColumnDescription) ? u.DbColumnName : u.ColumnDescription
            }).ToList();
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="className"></param>
        /// <param name="busName"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [NonAction]
        private async Task AddMenu(string className, string busName, long pid)
        {
            // 定义菜单编码前缀
            var codePrefix = "qingtian_" + className.ToLower();

            // 先删除该表已生成的菜单列表
            await _sysMenuRep.DeleteAsync(u => u.Code.StartsWith(codePrefix));

            // 如果 pid 为 0 说明为顶级菜单, 需要创建顶级目录
            if (pid == 0)
            {
                // 目录
                var menuType0 = new SysMenu
                {
                    Pid = 0,
                    Pids = $"[0],",
                    Name = busName + "管理",
                    Code = codePrefix,
                    Type = MenuType.MENU,
                    Icon = AntDesignIcons.RandomIcon,
                    Path = "/" + className.ToUnderscore(),
                    Component = "PageView"
                };
                menuType0 = await _sysMenuRep.InsertReturnEntityAsync(menuType0);
                pid = menuType0.Id;
            }
            // 由于后续菜单会有修改, 需要判断下 pid 是否存在, 不存在报错
            else if (!await _sysMenuRep.AnyAsync(e => e.Id == pid))
                throw Oops.Oh(ErrorCode.E1505);

            // 菜单
            var menuType1 = new SysMenu
            {
                Pid = pid,
                Pids = $"[0],[{pid}],",
                Name = busName + "管理",
                Code = codePrefix + "_mgr",
                Type = MenuType.MENU,
                Icon = AntDesignIcons.RandomIcon,
                Path = className.ToUnderscore(),
                Component = "/application/" + className.FirstCharToLower() + "/index",
                OpenType = MenuOpenType.COMPONENT
            };
            menuType1 = await _sysMenuRep.InsertReturnEntityAsync(menuType1);
            var pid1 = menuType1.Id;

            // 按钮-page
            var menuType2 = new SysMenu
            {
                Pid = pid1,
                Pids = $"[0],[{pid}],[{pid1}],",
                Name = busName + "查询",
                Code = codePrefix + "_mgr_page",
                Icon = AntDesignIcons.Search,
                Type = MenuType.BTN,
                Permission = className + ":page"
            };

            // 按钮-detail
            var menuType2_1 = new SysMenu
            {
                Pid = pid1,
                Pids = $"[0],[{pid}],[{pid1}],",
                Name = busName + "详情",
                Code = codePrefix + "_mgr_detail",
                Icon = AntDesignIcons.Detail,
                Type = MenuType.BTN,
                Permission = className + ":detail"
            };

            // 按钮-add
            var menuType2_2 = new SysMenu
            {
                Pid = pid1,
                Pids = $"[0],[{pid}],[{pid1}],",
                Name = busName + "增加",
                Code = codePrefix + "_mgr_add",
                Icon = AntDesignIcons.Add,
                Type = MenuType.BTN,
                Permission = className + ":add"
            };

            // 按钮-delete
            var menuType2_3 = new SysMenu
            {
                Pid = pid1,
                Pids = $"[0],[{pid}],[{pid1}],",
                Name = busName + "删除",
                Code = codePrefix + "_mgr_delete",
                Icon = AntDesignIcons.Delete,
                Type = MenuType.BTN,
                Permission = className + ":delete"
            };

            // 按钮-edit
            var menuType2_4 = new SysMenu
            {
                Pid = pid1,
                Pids = $"[0],[{pid}],[{pid1}],",
                Name = busName + "编辑",
                Code = codePrefix + "_mgr_edit",
                Icon = AntDesignIcons.Edit,
                Type = MenuType.BTN,
                Permission = className + ":edit"
            };

            List<SysMenu> menuList = new List<SysMenu>() { menuType2, menuType2_1, menuType2_2, menuType2_3, menuType2_4 };
            await _sysMenuRep.InsertAsync(menuList);
            // 清除缓存
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_MENU);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_ALLPERMISSION);
        }

        /// <summary>
        /// 设置生成文件路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        private List<string> GetTargetPathList(UpdateCodeGenParam param)
        {

            var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, "QingTian.Application", "Service", param.TableName);
            var servicePath = Path.Combine(backendPath, param.TableName + "Service.cs");
            var iservicePath = Path.Combine(backendPath, "I" + param.TableName + "Service.cs");
            var inputPath = Path.Combine(backendPath, "Param", param.TableName + "Param.cs");
            var outputPath = Path.Combine(backendPath, "Param", param.TableName + "Result.cs");
            var viewPath = Path.Combine(backendPath, "Param", param.TableName + "Put.cs");
            var RootPath = new DirectoryInfo(App.WebHostEnvironment.ContentRootPath);
            var FullName = "";
            // docker-compose frontend 目录位置
            if (RootPath.Name == "app")
            {
                FullName = "/app/";
            }
            else
            {
                FullName = RootPath.Parent.Parent.FullName;
            }
            var frontendPath = Path.Combine(FullName, "frontend", "src", "views", "application");
            var indexPath = Path.Combine(frontendPath, param.TableName.FirstCharToLower(), "index.vue");
            var FormPath = Path.Combine(frontendPath, param.TableName.FirstCharToLower(), param.TableName.FirstCharToLower() + "Form.vue");
            var FormDataPath = Path.Combine(frontendPath, param.TableName.FirstCharToLower(), param.TableName.FirstCharToLower() + ".data.ts");
            var apiJsPath = Path.Combine(FullName, "Frontend", "src", "api", "application", param.TableName.FirstCharToLower() + "Manage.ts");

            return new List<string>()
            {
                servicePath,
                iservicePath,
                inputPath,
                outputPath,
                viewPath,
                indexPath,
                FormPath,
                FormDataPath,
                apiJsPath
            };
        }

        /// <summary>
        /// 获取模板文件路径集合
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private List<string> GetTemplatePathList()
        {

            var templatePath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Template");
            return new List<string>()
            {
               Path.Combine(templatePath , "Service.cs.vm"),
                Path.Combine(templatePath , "IService.cs.vm"),
                Path.Combine(templatePath , "Param.cs.vm"),
                Path.Combine(templatePath , "Result.cs.vm"),
                Path.Combine(templatePath , "Put.cs.vm"),
                Path.Combine(templatePath , "index.vue.vm"),
                Path.Combine(templatePath , "Form.vue.vm"),
                Path.Combine(templatePath , "data.ts.vm"),
                Path.Combine(templatePath , "manage.ts.vm"),
            };
        }
        /// <summary>
        /// 根据实体名称获取表信息
        /// </summary>
        /// <param name="EntityName"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<string> GetTableByEntityName(string EntityName)
        {
            IEnumerable<EntityInfo> entityInfos = await _commonService.GetEntityInfos(true);
            foreach (var item in entityInfos)
            {
                if (item.EntityName == EntityName)
                {
                    return item.DbTableName;
                }
            }
            return EntityName;
        }
    }
}
