using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdateMenuParam : AddMenuParam
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required(ErrorMessage = "菜单Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>DeleteMenuInput
        [Required(ErrorMessage = "父级菜单Id不能为空")]
        public override long Pid { get; set; }
    }
}