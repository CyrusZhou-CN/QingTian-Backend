namespace QingTian.Core.Services
{
    public class RoleParam : ParamBase
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
        /// 数据范围类型: 1全部数据 2本部门及以下数据 3本部门数据 4仅本人数据 5自定义数据
        /// </summary>
        public DataScopeType DataScopeType { get; set; }

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