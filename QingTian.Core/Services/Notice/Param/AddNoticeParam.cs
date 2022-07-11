using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 添加通知消息参数
    /// </summary>
    public class AddNoticeParam: NoticeParam
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public override string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public override string Content { get; set; }

        /// <summary>
        /// 类型（字典 1通知 2消息）
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        public override NoticeType Type { get; set; }

        /// <summary>
        /// 状态: 0=草稿 1=发布 2=撤回 3=删除
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public override NoticeStatus Status { get; set; }

        /// <summary>
        /// 通知到的人
        /// </summary>
        [Required(ErrorMessage = "通知到的人不能为空")]
        public override List<long> NoticeUserIdList { get; set; }
    }
}