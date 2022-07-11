using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class DeleteUserParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }
    }
}