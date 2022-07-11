using System.Collections;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 登录菜单-AntDesign菜单类型
    /// </summary>
    public class MenuTreeNode : ITreeNode
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 路由名称, 必须设置,且不能重名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 重定向地址, 访问这个路由时, 自定进行重定向
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 路由元信息（路由附带扩展信息）
        /// </summary>
        public Meta Meta { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        public List<MenuTreeNode> Children { get; set; }
        public string Icon { get;  set; }
        public bool Disabled { get;  set; }

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
            Children = (List<MenuTreeNode>)children;
        }
    }
}