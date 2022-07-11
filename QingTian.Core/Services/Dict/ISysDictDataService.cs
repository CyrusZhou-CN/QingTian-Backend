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
    /// 字典值服务
    /// </summary>
    public interface ISysDictDataService
    {
        /// <summary>
        /// 增加字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddDictData(AddDictDataParam param);
        /// <summary>
        /// 修改字典值状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ChangeDictDataStatus(UpdateDictDataParam param);
        /// <summary>
        /// 删除字典下所有值
        /// </summary>
        /// <param name="dictTypeId"></param>
        /// <returns></returns>
        Task DeleteByTypeId(long dictTypeId);
        /// <summary>
        /// 删除字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteDictData(DeleteDictDataParam param);
        /// <summary>
        /// 字典值详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetDictData([FromQuery] QueryDictDataParam param);
        /// <summary>
        /// 获取某个字典类型下字典值列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetDictDataList([FromQuery] QueryDictDataListParam param);
        /// <summary>
        /// 根据字典类型Id获取字典值集合
        /// </summary>
        /// <param name="dictTypeId"></param>
        /// <returns></returns>
        Task<dynamic> GetDictDataListByDictTypeId(long dictTypeId);
        /// <summary>
        /// 分页查询字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryDictDataPageList([FromQuery] DictDataParam param);
        /// <summary>
        /// 更新字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateDictData(UpdateDictDataParam param);
        /// <summary>
        /// 根据字典Code返回字典值列表
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <param name="NetType"></param>
        /// <returns></returns>
        Task<List<dynamic>> GetDictDataByCode([FromQuery] string TypeCode, string NetType = "string");
    }
}
