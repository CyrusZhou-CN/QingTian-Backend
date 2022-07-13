using Mapster;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    public class HeaderNoticeReceiveView: IRegister
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime Datetime { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 允许点击
        /// </summary>
        public bool ClickClose { get; set; } = true;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 类型: 1=通知 2=消息
        /// </summary>
        public int Type { get; set; }
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<SysNotice, HeaderNoticeReceiveView>()
                  .Map(dest => dest.UserName, src => src.PublicUserName)
                  .Map(dest => dest.Datetime, src => src.PublicTime)
                  .Map(dest => dest.Description, src => src.Content.HtmlToText());
        }
    }
}