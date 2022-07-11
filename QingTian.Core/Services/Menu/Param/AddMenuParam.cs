using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddMenuParam: MenuParam
    {
        /// <summary>
        /// 菜单类型: 0=目录 1=菜单 2=按钮
        /// </summary>
        [Required(ErrorMessage = "菜单类型不能为空")]
        public override int Type { get; set; }
    }
}