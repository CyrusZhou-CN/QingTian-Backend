using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 通知消息服务
    /// </summary>
    public interface ISysNoticeService
    {
        /// <summary>
        /// 增加通知消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddNotice(AddNoticeParam param);
        /// <summary>
        /// 修改通知消息已读状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task SetReadStatus(QueryNoticeParam param);
        /// <summary>
        /// 修改通知消息状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ChangeStatus(ChangeStatusNoticeParam param);
        /// <summary>
        /// 删除通知消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteNotice(DeleteNoticeParam param);
        /// <summary>
        /// 获取通知消息详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NoticeDetailView> GetNotice([FromQuery] QueryNoticeParam param);
        /// <summary>
        /// 分页查询通知消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryNoticePageList([FromQuery] NoticeParam param);
        /// <summary>
        /// 获取接收的通知消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> ReceivedNoticePageList([FromQuery] NoticeParam param);
        /// <summary>
        /// 修改通知消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateNotice(UpdateNoticeParam param);
        /// <summary>
        /// 未处理消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> UnReadNoticeList([FromQuery] NoticeParam param);
    }
}
