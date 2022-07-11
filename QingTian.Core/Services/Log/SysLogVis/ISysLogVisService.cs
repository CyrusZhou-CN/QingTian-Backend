using QingTian.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 访问日志表服务
    /// </summary>
    public interface ISysLogVisService
    {
        /// <summary>
        /// 增加访问日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Add(AddSysLogVisParam param);
        /// <summary>
        /// 删除访问日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Delete(DeleteSysLogVisParam param);
        /// <summary>
        /// 获取访问日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysLogVis> Get([FromQuery] QueryeSysLogVisParam param);
        /// <summary>
        /// 获取访问日志表列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> List([FromQuery] SysLogVisParam param);
        /// <summary>
        /// 分页查询访问日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> Page([FromQuery] SysLogVisParam param);
        /// <summary>
        /// 更新访问日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Update(UpdateSysLogVisParam param);
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        Task ClearLog();
    }
}