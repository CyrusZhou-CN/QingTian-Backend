namespace QingTian.Core.Services
{
    /// <summary>
    /// 代码生成详细配置参数
    /// </summary>
    public class CodeGenConfig
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 代码生成主表ID
        /// </summary>
        public long CodeGenId { get; set; }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据库字段名(首字母小写)
        /// </summary>
        public string LowerColumnName => ColumnName.FirstCharToLower();

        /// <summary>
        /// 字段描述
        /// </summary>
        public string ColumnComment { get; set; }

        /// <summary>
        /// .NET类型
        /// </summary>
        public string NetType { get; set; }

        /// <summary>
        /// 作用类型（字典）
        /// </summary>
        public string EffectType { get; set; }

        /// <summary>
        /// 外键实体名称
        /// </summary>
        public string FkEntityName { get; set; }

        /// <summary>
        /// 外键实体名称(首字母小写)
        /// </summary>
        public string LowerFkEntityName => FkEntityName.FirstCharToLower();

        /// <summary>
        /// 外键显示字段
        /// </summary>
        public string FkColumnName { get; set; }

        /// <summary>
        /// 外键显示字段(首字母小写)
        /// </summary>
        public string LowerFkColumnName => FkColumnName.FirstCharToLower();

        /// <summary>
        /// 外键显示字段.NET类型
        /// </summary>
        public string FkColumnNetType { get; set; }

        /// <summary>
        /// 字典code
        /// </summary>
        public string DictTypeCode { get; set; }

        /// <summary>
        /// 列表是否缩进（字典）
        /// </summary>
        public bool WhetherRetract { get; set; }

        /// <summary>
        /// 是否必填（字典）
        /// </summary>
        public bool WhetherRequired { get; set; }

        /// <summary>
        /// 是否是查询条件
        /// </summary>
        public bool QueryWhether { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        /// 列表显示
        /// </summary>
        public bool WhetherTable { get; set; }

        /// <summary>
        /// 增改
        /// </summary>
        public bool WhetherAddUpdate { get; set; }

        /// <summary>
        /// 主外键
        /// </summary>
        public bool ColumnKey { get; set; }

        /// <summary>
        /// 数据库中类型（物理类型）
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否是通用字段
        /// </summary>
        public bool WhetherCommon { get; set; }
    }
}