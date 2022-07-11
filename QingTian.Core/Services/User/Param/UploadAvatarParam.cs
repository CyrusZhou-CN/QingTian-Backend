using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UploadAvatarParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 头像文件路径标识
        /// </summary>
        [Required(ErrorMessage = "头像文件路径标识不能为空")]
        public long Avatar { get; set; }
    }
}