using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class DeleteColumnParam
    {
        [Required(ErrorMessage = "表格名称不能为空")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "列名称不能为空")]
        public string DbColumnName { get; set; }
    }
}