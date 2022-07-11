using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdateNoticeParam : AddNoticeParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "通知消息Id不能为空")]
        public long Id { get; set; }
    }
}