using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="ISysDictTypeService"/>
    [Route("SysDictType"), ApiDescriptionSettings(Name = "DictType", Order = 100)]
    public class SysDictTypeService : ISysDictTypeService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysDictType> _sysDictTypeRep;  // 字典类型表
        private readonly ISysDictDataService _sysDictDataService;

        public SysDictTypeService(ISysDictDataService sysDictDataService,
                                  SqlSugarRepository<SysDictType> sysDictTypeRep)
        {
            _sysDictDataService = sysDictDataService;
            _sysDictTypeRep = sysDictTypeRep;
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddDictType(AddDictTypeParam param)
        {
            var isExist = await _sysDictTypeRep.AnyAsync(u => u.Name == param.Name || u.Code == param.Code);
            if (isExist) throw Oops.Oh(ErrorCode.E3001);

            var dictType = param.Adapt<SysDictType>();
            dictType = await _sysDictTypeRep.InsertReturnEntityAsync(dictType);
            if (param.DictDataList != null && param.DictDataList.Any())
            {
                foreach (var item in param.DictDataList)
                {
                    item.TypeId = dictType.Id;
                    await _sysDictDataService.AddDictData(item);
                }
            }
        }

        /// <inheritdoc/>
        [HttpPost("changeStatus")]
        public async Task ChangeDictTypeStatus(UpdateDictTypeParam param)
        {
            var dictType = await _sysDictTypeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (dictType == null) throw Oops.Oh(ErrorCode.E3000);

            if (!Enum.IsDefined(typeof(ValidityStatus), param.Status))
                throw Oops.Oh(ErrorCode.E3005);
            dictType.Status = param.Status;
            await _sysDictTypeRep.AsUpdateable(dictType).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteDictType(DeleteDictTypeParam param)
        {
            var dictType = await _sysDictTypeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (dictType == null) throw Oops.Oh(ErrorCode.E3000);
            await _sysDictTypeRep.DeleteAsync(dictType);
            await _sysDictDataService.DeleteByTypeId(param.Id);
        }

        /// <inheritdoc/>
        [AllowAnonymous]
        [HttpGet("tree")]
        public async Task<List<DictTreeView>> GetDictTree()
        {

            List<SysDictType> typeList = await GetDictTypeList();
            List<SysDictData> dataList = await _sysDictDataService.GetDictDataList(new QueryDictDataListParam());

            List<DictTreeView> list = new List<DictTreeView>();

            foreach (var item in typeList)
            {
                list.Add(new DictTreeView
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Children = dataList.Where(m => m.TypeId == item.Id).Select(m => new DictTreeView
                    {
                        Id = m.Id,
                        Pid = m.TypeId,
                        Code = m.Code,
                        Name = m.Value
                    }).ToList()
                });
            }
            return list;
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<dynamic> GetDictType([FromQuery] QueryDictTypeInfoParam param)
        {
            return await _sysDictTypeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [AllowAnonymous]
        [HttpGet("dropDown")]
        public async Task<dynamic> GetDictTypeDropDown([FromQuery] DropDownDictTypeParam param)
        {
            var dictType = await _sysDictTypeRep.FirstOrDefaultAsync(u => u.Code == param.Code);
            if (dictType == null) throw Oops.Oh(ErrorCode.E3000);
            return await _sysDictDataService.GetDictDataListByDictTypeId(dictType.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> GetDictTypeList()
        {
            return await _sysDictTypeRep.Where(u => u.Status != ValidityStatus.DELETED).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryDictTypePageList([FromQuery] DictTypeParam param)
        {
            var dictTypes = await _sysDictTypeRep.AsQueryable()
                 .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                 .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                 .Where(u => u.Status != ValidityStatus.DELETED).OrderBy(u => u.Sort)
                 .ToPagedListAsync(param.Page, param.PageSize);
            return dictTypes.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateDictType(UpdateDictTypeParam param)
        {
            var isExist = await _sysDictTypeRep.AnyAsync(u => u.Id == param.Id);
            if (!isExist) throw Oops.Oh(ErrorCode.E3000);

            // 排除自己并且判断与其他是否相同
            isExist = await _sysDictTypeRep.AnyAsync(u => (u.Name == param.Name || u.Code == param.Code) && u.Id != param.Id);
            if (isExist) throw Oops.Oh(ErrorCode.E3001);

            var dictType = param.Adapt<SysDictType>();
            await _sysDictTypeRep.AsUpdateable(dictType).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
    }
}
