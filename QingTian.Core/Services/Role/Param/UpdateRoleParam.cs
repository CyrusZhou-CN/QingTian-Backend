using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdateRoleParam : AddRoleParam
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色Id不能为空")]
        public long Id { get; set; }
    }
}