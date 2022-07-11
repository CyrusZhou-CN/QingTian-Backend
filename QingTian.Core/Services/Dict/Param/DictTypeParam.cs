namespace QingTian.Core.Services
{
    /// <summary>
    /// 字典类型参数
    /// </summary>
    public class DictTypeParam : PageParamBase
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
        public virtual int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public virtual ValidityStatus Status { get; set; }
    }
}