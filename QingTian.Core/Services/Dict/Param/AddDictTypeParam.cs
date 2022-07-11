using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddDictTypeParam:DictTypeParam
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "字典类型名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典类型编码不能为空")]
        public override string Code { get; set; }

        public List<AddDictDataParam> DictDataList { get; set; }
    }
}