using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class DeletePositionParam
    {
        /// <summary>
        /// 职位Id
        /// </summary>
        [Required(ErrorMessage = "职位Id不能为空")]
        public long Id { get; set; }
    }
}