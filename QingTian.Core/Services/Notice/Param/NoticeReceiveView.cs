namespace QingTian.Core.Services
{
    /// <summary>
    /// 通知消息接收参数
    /// </summary>
    public class NoticeReceiveView : NoticeBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 阅读状态（字典 0未读 1已读）
        /// </summary>
        public int ReadStatus { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime ReadTime { get; set; }
    }
}