using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdateOrganizeParam : AddOrganizeParam
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        [Required(ErrorMessage = "机构Id不能为空")]
        public string Id { get; set; }
    }
}