using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdatePasswordUserParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(NewPassword), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }
    }
}