namespace QingTian.Core.Services
{
    /// <summary>
    /// 职位参数
    /// </summary>
    public class PositionParam
    {

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}