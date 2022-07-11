namespace QingTian.Core.Services
{
    /// <summary>
    /// 数据库表列表参数
    /// </summary>
    public class TableParam
    {
        /// <summary>
        /// 表名（字母形式的）
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }
    }
}