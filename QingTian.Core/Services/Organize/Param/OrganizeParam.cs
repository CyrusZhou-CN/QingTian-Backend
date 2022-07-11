namespace QingTian.Core.Services
{
    /// <summary>
    /// 组织机构参数
    /// </summary>
    public class OrganizeParam : ParamBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 父Ids
        /// </summary>
        public string Pids { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel { get; set; }

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
    }
}