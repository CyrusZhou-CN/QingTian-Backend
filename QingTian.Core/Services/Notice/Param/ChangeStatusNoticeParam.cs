using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 修改通知消息状态参数
    /// </summary>
    public class ChangeStatusNoticeParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "通知消息Id不能为空")]
        public long Id { get; set; }
        /// <summary>
        /// 状态: 0=草稿 1=发布 2=撤回 3=删除
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public NoticeStatus Status { get; set; }
    }
}