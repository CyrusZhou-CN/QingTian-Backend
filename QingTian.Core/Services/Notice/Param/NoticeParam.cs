namespace QingTian.Core.Services
{
    /// <summary>
    /// 通知消息参数
    /// </summary>
    public class NoticeParam: PageParamBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 类型: 1=通知 2=消息
        /// </summary>
        public virtual NoticeType Type { get; set; }

        /// <summary>
        /// 状态: 0=草稿 1=发布 2=撤回 3=删除
        /// </summary>
        public virtual NoticeStatus Status { get; set; }

        /// <summary>
        /// 通知到的人
        /// </summary>
        public virtual List<long> NoticeUserIdList { get; set; }
    }
}