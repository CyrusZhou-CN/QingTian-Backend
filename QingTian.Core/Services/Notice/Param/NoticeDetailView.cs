namespace QingTian.Core.Services
{
    public class NoticeDetailView: NoticeBase
    {
        /// <summary>
             /// 通知到的用户Id集合
             /// </summary>
        public List<string> NoticeUserIdList { get; set; }

        /// <summary>
        /// 通知到的用户阅读信息集合
        /// </summary>
        public List<NoticeUserRead> NoticeUserReadInfoList { get; set; }
    }
}