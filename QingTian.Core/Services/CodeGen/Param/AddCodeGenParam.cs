using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddCodeGenParam : CodeGenParam
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        [Required(ErrorMessage = "数据库表名不能为空")]
        public override string TableName { get; set; }

        /// <summary>
        /// 业务名（业务代码包名称）
        /// </summary>
        [Required(ErrorMessage = "业务名不能为空")]
        public override string BusName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Required(ErrorMessage = "命名空间不能为空")]
        public override string NameSpace { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [Required(ErrorMessage = "作者姓名不能为空")]
        public override string AuthorName { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        [Required(ErrorMessage = "生成方式不能为空")]
        public override GenerateType GenerateType { get; set; }
    }
}