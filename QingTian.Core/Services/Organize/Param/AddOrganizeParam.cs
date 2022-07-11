using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddOrganizeParam : OrganizeParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "机构名称不能为空")]
        public override string Name { get; set; }
    }
}