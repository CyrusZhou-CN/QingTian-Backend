using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 代码生成详细配置服务
    /// </summary>
    public interface ICodeGenConfigService
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Add(CodeGenConfig param);

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="tableColumnResultList"></param>
        /// <param name="codeGenerate"></param>
        void AddList(List<TableColumnResult> tableColumnResultList, SysCodeGen codeGenerate);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="codeGenId"></param>
        /// <returns></returns>
        Task Delete(long codeGenId);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysCodeGenConfig> Detail(CodeGenConfig param);

        /// <summary>
        /// 代码生成详细配置列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<CodeGenConfig>> List([FromQuery] CodeGenConfig param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        Task Update(List<CodeGenConfig> paramList);
    }
}
