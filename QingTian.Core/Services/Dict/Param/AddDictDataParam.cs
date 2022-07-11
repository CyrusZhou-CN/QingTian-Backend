using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddDictDataParam : DictDataParam
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public override long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required(ErrorMessage = "字典值不能为空")]
        public override string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典值编码不能为空")]
        public override string Code { get; set; }
    }
}