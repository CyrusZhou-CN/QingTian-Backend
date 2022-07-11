using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 删除通知消息参数
    /// </summary>
    public class DeleteNoticeParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "通知消息Id不能为空")]
        public long Id { get; set; }
    }
}