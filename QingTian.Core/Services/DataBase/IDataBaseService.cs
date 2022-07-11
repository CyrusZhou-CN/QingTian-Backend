using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 数据库管理服务
    /// </summary>
    public interface IDataBaseService
    {
        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetTableInfoList([FromQuery] DbPageParam pageParam);

        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<dynamic> GetColumnInfosByTableName([FromQuery] DbPageParam pageParam);

        /// <summary>
        /// 新增表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task TableAdd(DbTableInfoParam param);

        /// <summary>
        /// 编辑表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task TableEdit(EditTableParam param);

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task TableDelete(DbTableInfo param);

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="param"></param>
        Task ColumnAdd(DbColumnInfoParam param);

        /// <summary>
        /// 编辑列
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ColumnEdit(EditColumnParam param);

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ColumnDelete(DeleteColumnParam param);

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateEntity(CreateEntityParam param);
    }
}
