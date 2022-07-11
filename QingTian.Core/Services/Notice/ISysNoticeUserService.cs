using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 通知消息用户
    /// </summary>
    public interface ISysNoticeUserService
    {
        /// <summary>
        /// 增加通知消息用户
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="noticeUserIdList"></param>
        /// <param name="noticeUserStatus"></param>
        /// <returns></returns>
        Task AddAsync(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus);

        /// <summary>
        /// 获取通知消息用户列表
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        Task<List<SysNoticeUser>> GetNoticeUserListByNoticeIdAsync(long noticeId);

        /// <summary>
        /// 设置通知消息读取状态
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SetReadStatusAsync(long noticeId, long userId, NoticeUserStatus status);

        /// <summary>
        /// 修改通知消息用户
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="noticeUserIdList"></param>
        /// <param name="noticeUserStatus"></param>
        /// <returns></returns>
        Task UpdateAsync(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus);
    }
}