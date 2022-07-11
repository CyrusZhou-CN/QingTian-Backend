namespace QingTian.Core.Services
{
    public class NoticeUserRead
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 状态: 0=未读 1=已读
        /// </summary>
        public NoticeUserStatus ReadStatus { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime ReadTime { get; set; }
    }
}