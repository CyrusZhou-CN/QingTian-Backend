using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 代码生成表
    /// </summary>
    [SugarTable("sys_code_gen", TableDescription = "代码生成表")]
    [Description("代码生成表")]
    public class SysCodeGen : DbEntityBase
    {
        /// <summary>
        /// 作者姓名
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "作者姓名")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        [MaxLength(5)]
        [SugarColumn(ColumnDescription = "是否移除表前缀", IsNullable = true)]
        public string TablePrefix { get; set; }

        /// <summary>
        /// 生成方式: 项目目录=0 下载项目=1
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "生成方式")]
        public GenerateType GenerateType { get; set; } = GenerateType.ProjectDirectory;

        /// <summary>
        /// 数据库表名
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(ColumnDescription = "数据库表名")]
        public string TableName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(ColumnDescription = "命名空间")]
        public string NameSpace { get; set; }

        /// <summary>
        /// 业务名
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(ColumnDescription = "业务名")]
        public string BusName { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单编码", IsNullable = true)]
        public long MenuPid { get; set; }
    }
}