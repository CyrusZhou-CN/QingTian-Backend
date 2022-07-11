using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 菜单树---授权、新增编辑时选择
    /// </summary>
    public class MenuTreeView : ITreeNode
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 排序，越小优先级越高
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuTreeView> Children { get; set; }

        /// <summary>
        /// 引用排序
        /// </summary>
        [JsonIgnore]
        public int AppSort { get; set; }

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return ParentId;
        }

        public void SetChildren(IList children)
        {
            Children = (List<MenuTreeView>)children;
        }
    }
}
