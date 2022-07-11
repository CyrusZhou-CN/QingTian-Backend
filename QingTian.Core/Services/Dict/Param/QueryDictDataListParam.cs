using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class QueryDictDataListParam
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public long TypeId { get; set; }
    }
}