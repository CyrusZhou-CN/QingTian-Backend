namespace QingTian.Core.Services
{
    /// <summary>
    /// 通知消息参数
    /// </summary>
    public class NoticeBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型: 1=通知 2=消息
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        public long PublicUserId { get; set; }

        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string PublicUserName { get; set; }

        /// <summary>
        /// 发布机构Id
        /// </summary>
        public long PublicOrgId { get; set; }

        /// <summary>
        /// 发布机构名称
        /// </summary>
        public string PublicOrgName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublicTime { get; set; }

        /// <summary>
        /// 撤回时间
        /// </summary>
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 状态: 0=草稿 1=发布 2=撤回 3=删除
        /// </summary>
        public int Status { get; set; }
    }
}