using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.CodeGen
{
    /// <inheritdoc cref="ICodeGenConfigService"/>
    [Route("SysCodeGenerateConfig"), ApiDescriptionSettings(Name = "CodeGenConfig", Order = 998)]
    public class CodeGenConfigService : ICodeGenConfigService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysCodeGenConfig> _sysCodeGenConfigRep; // 代码生成详细配置

        public CodeGenConfigService(SqlSugarRepository<SysCodeGenConfig> sysCodeGenConfigRep)
        {
            _sysCodeGenConfigRep = sysCodeGenConfigRep;
        }


        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysCodeGenConfig> Detail(CodeGenConfig param)
        {
            return await _sysCodeGenConfigRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<List<CodeGenConfig>> List([FromQuery] CodeGenConfig param)
        {
            return await _sysCodeGenConfigRep.Where(u => u.CodeGenId == param.CodeGenId && u.WhetherCommon != YesOrNo.Yes.ToBool())
              .Select<CodeGenConfig>().ToListAsync();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task Update(List<CodeGenConfig> paramList)
        {
            if (paramList == null || paramList.Count < 1) return;
            List<SysCodeGenConfig> list = paramList.Adapt<List<SysCodeGenConfig>>();
            await _sysCodeGenConfigRep.UpdateAsync(list);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task Add(CodeGenConfig param)
        {
            var codeGenConfig = param.Adapt<SysCodeGenConfig>();
            await _sysCodeGenConfigRep.InsertAsync(codeGenConfig);
        }

        /// <inheritdoc/>
        [NonAction]
        public void AddList(List<TableColumnResult> tableColumnResultList, SysCodeGen codeGenerate)
        {

            if (tableColumnResultList == null) return;

            var codeGenConfigs = new List<SysCodeGenConfig>();

            foreach (var tableColumn in tableColumnResultList)
            {
                var codeGenConfig = new SysCodeGenConfig();

                var _yesOrNo = YesOrNo.Yes.ToBool();
                if (tableColumn.ColumnKey)
                {
                    _yesOrNo = YesOrNo.No.ToBool();
                }

                if (CodeGenUtil.IsCommonColumn(tableColumn.ColumnName))
                {
                    codeGenConfig.WhetherCommon = YesOrNo.Yes.ToBool();
                    _yesOrNo = YesOrNo.No.ToBool();
                }
                else
                {
                    codeGenConfig.WhetherCommon = YesOrNo.No.ToBool();
                }

                codeGenConfig.CodeGenId = codeGenerate.Id;
                codeGenConfig.ColumnName = tableColumn.ColumnName;
                codeGenConfig.ColumnComment = tableColumn.ColumnComment;
                codeGenConfig.NetType = CodeGenUtil.ConvertDataType(tableColumn.DataType);
                codeGenConfig.WhetherRetract = YesOrNo.No.ToBool();

                codeGenConfig.WhetherRequired = YesOrNo.No.ToBool();
                codeGenConfig.QueryWhether = _yesOrNo;
                codeGenConfig.WhetherAddUpdate = _yesOrNo;
                codeGenConfig.WhetherTable = _yesOrNo;

                codeGenConfig.ColumnKey = tableColumn.ColumnKey;

                codeGenConfig.DataType = tableColumn.DataType;
                codeGenConfig.EffectType = CodeGenUtil.DataTypeToEff(codeGenConfig.NetType);
                codeGenConfig.QueryType = "=="; 
                codeGenConfigs.Add(codeGenConfig);
            }
            _sysCodeGenConfigRep.InsertAsync(codeGenConfigs);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task Delete(long codeGenId)
        {
            await _sysCodeGenConfigRep.DeleteAsync(u => u.CodeGenId == codeGenId);
        }
    }
}
