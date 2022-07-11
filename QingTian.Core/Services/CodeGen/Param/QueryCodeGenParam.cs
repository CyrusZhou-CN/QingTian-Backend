using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class QueryCodeGenParam
    {
        /// <summary>
        /// 代码生成器Id
        /// </summary>
        [Required(ErrorMessage = "代码生成器Id不能为空")]
        public long Id { get; set; }
    }
}