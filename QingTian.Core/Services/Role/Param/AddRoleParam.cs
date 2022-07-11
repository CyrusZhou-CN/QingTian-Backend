using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddRoleParam: RoleParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空")]
        public override string Code { get; set; }
    }
}