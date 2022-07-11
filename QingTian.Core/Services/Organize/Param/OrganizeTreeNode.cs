using System.Collections;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 组织机构树
    /// </summary>
    public class OrganizeTreeNode : ITreeNode
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 上级 Id
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序(升序)
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public ValidityStatus Status { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<OrganizeTreeNode> Children { get; set; }

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return Pid;
        }

        public void SetChildren(IList children)
        {
            Children = (List<OrganizeTreeNode>)children;
        }
    }
}