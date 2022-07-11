using QingTian.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 操作日志表服务
    /// </summary>
    public interface ISysLogOpService
    {
        /// <summary>
        /// 增加操作日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Add(AddSysLogOpParam param);
        /// <summary>
        /// 删除操作日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Delete(DeleteSysLogOpParam param);
        /// <summary>
        /// 获取操作日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysLogOp> Get([FromQuery] QueryeSysLogOpParam param);
        /// <summary>
        /// 获取操作日志表列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> List([FromQuery] SysLogOpParam param);
        /// <summary>
        /// 分页查询操作日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> Page([FromQuery] SysLogOpParam param);
        /// <summary>
        /// 更新操作日志表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Update(UpdateSysLogOpParam param);
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        Task ClearLog();
    }
}