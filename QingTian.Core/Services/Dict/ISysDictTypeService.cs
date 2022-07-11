using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 字典类型服务
    /// </summary>
    public interface ISysDictTypeService
    {
        /// <summary>
        /// 添加字典类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddDictType(AddDictTypeParam param);
        /// <summary>
        /// 更新字典类型状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ChangeDictTypeStatus(UpdateDictTypeParam param);
        /// <summary>
        /// 删除字典类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteDictType(DeleteDictTypeParam param);
        /// <summary>
        /// 字典类型与字典值构造的字典树
        /// </summary>
        /// <returns></returns>
        Task<List<DictTreeView>> GetDictTree();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetDictType([FromQuery] QueryDictTypeInfoParam param);
        /// <summary>
        /// 获取字典类型下所有字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetDictTypeDropDown([FromQuery] DropDownDictTypeParam param);
        /// <summary>
        /// 获取字典类型列表
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetDictTypeList();
        /// <summary>
        /// 分页查询字典类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryDictTypePageList([FromQuery] DictTypeParam param);
        /// <summary>
        /// 更新字典类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateDictType(UpdateDictTypeParam param);
    }
}
