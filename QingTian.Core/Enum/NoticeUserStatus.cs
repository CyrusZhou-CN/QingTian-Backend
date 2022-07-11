using System.ComponentModel;

namespace QingTian.Core
{
    /// <summary>
    /// 通知消息用户状态
    /// </summary>
    public enum NoticeUserStatus
    {
        /// <summary>
        /// 未读
        /// </summary>
        [Description("未读")]
        UNREAD = 0,

        /// <summary>
        /// 已读
        /// </summary>
        [Description("已读")]
        READ = 1
    }
}