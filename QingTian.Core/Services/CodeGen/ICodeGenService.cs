using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.CodeGen
{
    /// <summary>
    /// 代码生成器服务
    /// </summary>
    public interface ICodeGenService
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddCodeGen(AddCodeGenParam param);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteCodeGen(List<DeleteCodeGenParam> param);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenParam param);

        /// <summary>
        /// 获取数据表列（实体属性）集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<TableColumnResult>> GetColumnList(AddCodeGenParam param);

        /// <summary>
        /// 根据表名获取列
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<TableColumnResult>> GetColumnListByTableParam(TableParam param);
        /// <summary>
        /// 获取数据库表(实体)集合
        /// </summary>
        /// <returns></returns>
        Task<List<TableResult>> GetTableList();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryCodeGenPageList([FromQuery] CodeGenPageParam param);

        /// <summary>
        /// 代码生成 本地项目
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task RunLocal(UpdateCodeGenParam param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateCodeGen(UpdateCodeGenParam param);

    }
}
