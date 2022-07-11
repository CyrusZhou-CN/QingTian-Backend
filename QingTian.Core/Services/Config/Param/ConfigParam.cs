namespace QingTian.Core.Services
{
    /// <summary>
    /// 参数配置
    /// </summary>
    public class ConfigParam : PageParamBase
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
        /// 属性值
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 是否是系统参数（1-是，0-否）
        /// </summary>
        public virtual int SysFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 常量所属分类的编码，来自于“常量的分类”字典
        /// </summary>
        public virtual string GroupCode { get; set; }
    }
}