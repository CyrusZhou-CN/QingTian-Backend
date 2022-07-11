using QingTian.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 异常日志服务
    /// </summary>
    public interface ISysLogExService
    {
        /// <summary>
        /// 增加异常日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Add(AddSysLogExParam param);
        /// <summary>
        /// 删除异常日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Delete(DeleteSysLogExParam param);
        /// <summary>
        /// 获取异常日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysLogEx> Get([FromQuery] QueryeSysLogExParam param);
        /// <summary>
        /// 获取异常日志列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> List([FromQuery] SysLogExParam param);
        /// <summary>
        /// 分页查询异常日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> Page([FromQuery] SysLogExParam param);
        /// <summary>
        /// 更新异常日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Update(UpdateSysLogExParam param);
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        Task ClearLog();
    }
}