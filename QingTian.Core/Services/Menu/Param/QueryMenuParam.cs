using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class QueryMenuParam
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required(ErrorMessage = "菜单Id不能为空")]
        public long Id { get; set; }
    }
}