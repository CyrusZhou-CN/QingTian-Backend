using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="ISysDictDataService"/>
    [Route("SysDictData"), ApiDescriptionSettings(Name = "DictData", Order = 100)]
    public class SysDictDataService : ISysDictDataService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysDictData> _sysDictDataRep;  // 字典类型表

        public SysDictDataService(SqlSugarRepository<SysDictData> sysDictDataRep)
        {
            _sysDictDataRep = sysDictDataRep;
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddDictData(AddDictDataParam param)
        {
            var isExist = await _sysDictDataRep.AnyAsync(u => (u.Code == param.Code || u.Value == param.Value) && u.TypeId == param.TypeId);
            if (isExist) throw Oops.Oh(ErrorCode.E3003);

            var dictData = param.Adapt<SysDictData>();
            await _sysDictDataRep.InsertAsync(dictData);
        }

        /// <inheritdoc/>
        [HttpPost("changeStatus")]
        public async Task ChangeDictDataStatus(UpdateDictDataParam param)
        {
            var dictData = await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (dictData == null) throw Oops.Oh(ErrorCode.E3004);

            if (!Enum.IsDefined(typeof(ValidityStatus), param.Status))
                throw Oops.Oh(ErrorCode.E3005);
            dictData.Status = param.Status;
        }


        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteDictData(DeleteDictDataParam param)
        {
            var dictData = await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (dictData == null) throw Oops.Oh(ErrorCode.E3004);

            await _sysDictDataRep.DeleteAsync(dictData);
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<dynamic> GetDictData([FromQuery] QueryDictDataParam param)
        {
            return await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> GetDictDataList([FromQuery] QueryDictDataListParam param)
        {
            return await _sysDictDataRep.AsQueryable()
               .WhereIF(param.TypeId != 0, u => u.TypeId == param.TypeId)
               .Where(u => u.Status != ValidityStatus.DELETED).OrderBy(u => u.Sort).ToListAsync();
        }
        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryDictDataPageList([FromQuery] DictDataParam param)
        {
            var code = !string.IsNullOrEmpty(param.Code?.Trim());
            var value = !string.IsNullOrEmpty(param.Value?.Trim());
            var dictDatas = await _sysDictDataRep.AsQueryable()
                .Where(u => u.TypeId == param.TypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                .WhereIF(!string.IsNullOrWhiteSpace(param.Value), u => u.Code.Contains(param.Value.Trim()))
                .Where(u => u.Status != ValidityStatus.DELETED).OrderBy(u => u.Sort)
                .Select<DictDataView>()
                .ToPagedListAsync(param.Page, param.PageSize);
            return dictDatas.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpGet("dict_data_by_code")]
        public async Task<List<dynamic>> GetDictDataByCode([FromQuery] string TypeCode, string NetType = "string")
        {
            var list = await _sysDictDataRep.AsQueryable().LeftJoin<SysDictType>((d, t) => d.TypeId == t.Id).Where((d, t) => t.Code == TypeCode && t.Status != ValidityStatus.DELETED && d.Status != ValidityStatus.DELETED).OrderBy(d => d.Sort).Select<SysDictData>().ToListAsync();
            List<dynamic> dictDatas = new List<dynamic>();
            foreach (var item in list)
            {
                dictDatas.Add(new { Label = item.Value, Value = item.Code });
            }
            return dictDatas;
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateDictData(UpdateDictDataParam param)
        {
            var isExist = await _sysDictDataRep.AnyAsync(u => u.Id == param.Id);
            if (!isExist) throw Oops.Oh(ErrorCode.E3004);

            // 排除自己并且判断与其他是否相同
            isExist = await _sysDictDataRep.AnyAsync(u => (u.Value == param.Value || u.Code == param.Code) && u.TypeId == param.TypeId && u.Id != param.Id);
            if (isExist) throw Oops.Oh(ErrorCode.E3003);
            var dictData = await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            dictData = param.Adapt(dictData);
            await _sysDictDataRep.AsUpdateable(dictData).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task<dynamic> GetDictDataListByDictTypeId(long dictTypeId)
        {
            return await _sysDictDataRep.Where(u => u.TypeId == dictTypeId)
                                                        .Where(u => u.Status != ValidityStatus.DELETED).OrderBy(u => u.Sort)
                                                        .Select(u => new
                                                        {
                                                            u.Code,
                                                            u.Value
                                                        }).ToListAsync();
        }
        /// <inheritdoc/>
        [NonAction]
        public async Task DeleteByTypeId(long dictTypeId)
        {
            await _sysDictDataRep.DeleteAsync(u => u.TypeId == dictTypeId);
        }
    }
}
