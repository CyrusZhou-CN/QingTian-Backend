using Furion;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.ViewEngine;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.DataBase
{
    /// <inheritdoc cref="IDataBaseService"/>
    [Route("DataBase"), ApiDescriptionSettings(Name = "DataBase", Order = 200)]
    public class DataBaseService : IDataBaseService, IDynamicApiController, ITransient
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly IViewEngine _viewEngine;
        public DataBaseService(ISqlSugarClient sqlSugarClient, IViewEngine viewEngine)
        {
            _sqlSugarClient = sqlSugarClient;
            _viewEngine = viewEngine;
        }

        /// <inheritdoc/>
        [HttpPost("Table/Column/add")]
        public async Task ColumnAdd(DbColumnInfoParam param)
        {
            await Task.Run(() =>
            {
                DbColumnInfo column = new DbColumnInfo();
                column.ColumnDescription = param.ColumnDescription;
                column.DbColumnName = param.DbColumnName;
                column.IsIdentity = param.IsIdentity == 1;
                column.IsNullable = param.IsNullable == 1;
                column.IsPrimarykey = param.IsPrimarykey == 1;
                column.Length = param.Length;
                column.DecimalDigits = param.DecimalDigits;
                column.DefaultValue = param.DefaultValue;
                column.DataType = param.DataType;
                _sqlSugarClient.DbMaintenance.AddColumn(param.TableName, column);
            });
        }

        /// <inheritdoc/>
        [HttpPost("Table/Column/delete")]
        public async Task ColumnDelete(DeleteColumnParam param)
        {
            await Task.Run(() =>
            {
                _sqlSugarClient.DbMaintenance.DropColumn(param.TableName, param.DbColumnName);
            });
        }

        /// <inheritdoc/>
        [HttpPost("Table/Column/edit")]
        public async Task ColumnEdit(EditColumnParam param)
        {
            await Task.Run(() =>
            {
                _sqlSugarClient.DbMaintenance.RenameColumn(param.TableName, param.OldName, param.DbColumnName);
                if (_sqlSugarClient.DbMaintenance.IsAnyColumnRemark(param.DbColumnName, param.TableName))
                {
                    _sqlSugarClient.DbMaintenance.DeleteColumnRemark(param.DbColumnName, param.TableName);
                }
                _sqlSugarClient.DbMaintenance.AddColumnRemark(param.DbColumnName, param.TableName, string.IsNullOrWhiteSpace(param.ColumnDescription) ? param.DbColumnName : param.ColumnDescription);
            });
        }

        /// <inheritdoc/>
        [HttpGet("Table/Column/InfoList")]
        public async Task<dynamic> GetColumnInfosByTableName([FromQuery] DbPageParam pageParam)
        {
            if (string.IsNullOrWhiteSpace(pageParam.TableName))
                return new SqlSugarPagedList<DbColumnInfoResult>();
            if (pageParam.Pagination)
            {
                var list = await _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(pageParam.TableName, false).ToPagedListAsync(pageParam.Page, pageParam.PageSize);
                return list.QtPagedResult();
            }
            else
            {
                return _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(pageParam.TableName, false);
            }
        }

        /// <inheritdoc/>
        [HttpPost("Table/createEntity")]
        public async Task CreateEntity(CreateEntityParam param)
        {
            await Task.Run(() =>
            {
                Type baseType = Type.GetType($"QingTian.Core.Entity.{param.BaseClassName}");

                if (baseType.IsNullOrZero())
                    throw Oops.Oh(ErrorCode.db1003);

                Type typeEntityName = Type.GetType($"QingTian.Core.Entity.{param.EntityName}");
                if (!typeEntityName.IsNullOrZero())
                {
                    throw Oops.Oh(ErrorCode.db1002);
                }
                var baseTypeProperties = baseType.GetProperties().Select(m => m.Name).ToList();

                param.Position = string.IsNullOrWhiteSpace(param.Position) ? "QingTian.Application" : param.Position;
                var templatePath = GetTemplatePath();
                var targetPath = GetTargetPath(param);
                DbTableInfo dbTableInfo = _sqlSugarClient.DbMaintenance.GetTableInfoList(false).FirstOrDefault(m => m.Name == param.TableName);
                if (dbTableInfo == null)
                    throw Oops.Oh(ErrorCode.db1001);
                List<DbColumnInfo> dbColumnInfos = _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(param.TableName, false).Where(m => !baseTypeProperties.Contains(m.DbColumnName)).ToList();
                dbColumnInfos.ForEach(m =>
                {
                    m.DataType = CodeGenUtil.ConvertDataType(m.DataType);
                });
                var tContent = File.ReadAllText(templatePath);
                var tResult = _viewEngine.RunCompileFromCached(tContent, new
                {
                    param.TableName,
                    param.EntityName,
                    param.BaseClassName,
                    dbTableInfo.Description,
                    TableField = dbColumnInfos
                });

                #region 检查目录是否存在
                FileInfo fileInfo = new FileInfo(targetPath);
                string entityPath = fileInfo.DirectoryName;
                if (!Directory.Exists(entityPath))
                {
                    Directory.CreateDirectory(entityPath);
                }
                #endregion

                File.WriteAllText(targetPath, tResult, Encoding.UTF8);
            });
        }

        /// <inheritdoc/>
        [HttpGet("Table/InfoList")]
        public async Task<dynamic> GetTableInfoList([FromQuery] DbPageParam pageParam)
        {
            if (pageParam.Pagination)
            {
                var list = await _sqlSugarClient.DbMaintenance.GetTableInfoList(false).ToPagedListAsync(pageParam.Page, pageParam.PageSize);
                return list.QtPagedResult();
            }
            else
            {
                return _sqlSugarClient.DbMaintenance.GetTableInfoList(false);
            }
        }

        /// <inheritdoc/>
        [HttpPost("Table/add")]
        public async Task TableAdd(DbTableInfoParam param)
        {
            await Task.Run(() =>
            {
                List<DbColumnInfo> columns = new List<DbColumnInfo>();
                if (param.DbColumnInfoList == null || !param.DbColumnInfoList.Any())
                {
                    throw Oops.Oh(ErrorCode.db1000);
                }
                param.DbColumnInfoList.ForEach(m =>
                {
                    columns.Add(new DbColumnInfo
                    {
                        DbColumnName = m.DbColumnName,
                        DataType = m.DataType,
                        Length = m.Length,
                        DefaultValue= m.DefaultValue,
                        ColumnDescription = m.ColumnDescription,
                        IsNullable = m.IsNullable == 1,
                        IsIdentity = m.IsIdentity == 1,
                        IsPrimarykey = m.IsPrimarykey == 1,
                        DecimalDigits = m.DecimalDigits
                    });
                });
                _sqlSugarClient.DbMaintenance.CreateTable(param.Name, columns, false);
                _sqlSugarClient.DbMaintenance.AddTableRemark(param.Name, param.Description);

                if (columns.Any(m => m.IsPrimarykey))
                {
                    _sqlSugarClient.DbMaintenance.AddPrimaryKey(param.Name, columns.FirstOrDefault(m => m.IsPrimarykey).DbColumnName, columns.FirstOrDefault(m => m.IsPrimarykey).IsIdentity);
                }
                param.DbColumnInfoList.ForEach(m =>
                {
                    _sqlSugarClient.DbMaintenance.AddColumnRemark(m.DbColumnName, param.Name, string.IsNullOrWhiteSpace(m.ColumnDescription) ? m.DbColumnName : m.ColumnDescription);
                });
            });
        }

        /// <inheritdoc/>
        [HttpPost("Table/delete")]
        public async Task TableDelete(DbTableInfo param)
        {
            await Task.Run(() =>
            {
                _sqlSugarClient.DbMaintenance.DropTable(param.Name);
            });
        }

        /// <inheritdoc/>
        [HttpPost("Table/edit")]
        public async Task TableEdit(EditTableParam param)
        {
            await Task.Run(() =>
            {
                _sqlSugarClient.DbMaintenance.RenameTable(param.OldName, param.Name);
                if (_sqlSugarClient.DbMaintenance.IsAnyTableRemark(param.Name))
                {
                    _sqlSugarClient.DbMaintenance.DeleteTableRemark(param.Name);
                }
                _sqlSugarClient.DbMaintenance.AddTableRemark(param.Name, param.Description);
            });
        }

        /// <summary>
        /// 获取模板文件路径集合
        /// </summary>
        /// <returns></returns>
        private string GetTemplatePath()
        {
            var templatePath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Template");
            return Path.Combine(templatePath, "Entity.cs.vm");
        }

        /// <summary>
        /// 设置生成文件路径
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetTargetPath([FromQuery] CreateEntityParam param)
        {
            var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, param.Position, "Entity");
            var entityPath = Path.Combine(backendPath, param.EntityName + ".cs");
            return entityPath;
        }
    }
}
