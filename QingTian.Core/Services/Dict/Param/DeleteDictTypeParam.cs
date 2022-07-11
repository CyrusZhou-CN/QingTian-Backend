using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class DeleteDictTypeParam
    {
        /// <summary>
        /// 编号Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public long Id { get; set; }
    }
}