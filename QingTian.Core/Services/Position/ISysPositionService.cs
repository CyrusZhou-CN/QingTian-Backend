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
    /// 职位服务
    /// </summary>
    /// 
    public interface ISysPositionService
    {
        /// <summary>
        /// 增加职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddPosition(AddPositionParam param);
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeletePosition(DeletePositionParam param);
        /// <summary>
        /// 获取职位详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysPosition> GetPosition([FromQuery] QueryPositionParam param);
        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetPositionList([FromQuery] PositionParam param);
        /// <summary>
        /// 分页获取职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryPositionPageList([FromQuery] PositionParam param);
        /// <summary>
        /// 更新职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdatePosition(UpdatePositionParam param);
    }
}
