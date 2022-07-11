using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdateDictDataParam : AddDictDataParam
    {
        /// <summary>
        /// 字典值Id
        /// </summary>
        [Required(ErrorMessage = "字典值Id不能为空")]
        public long Id { get; set; }
    }
}