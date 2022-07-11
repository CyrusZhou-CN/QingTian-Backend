using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class DropDownDictTypeParam
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典类型编码不能为空")]
        public string Code { get; set; }
    }
}