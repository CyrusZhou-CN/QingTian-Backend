using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    public class MenuView : MenuParam, ITreeNode
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuView> Children { get; set; }

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
            Children = (List<MenuView>)children;
        }
    }
}
