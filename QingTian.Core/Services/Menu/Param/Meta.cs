namespace QingTian.Core.Services
{
    /// <summary>
    /// 路由元信息内部类
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// 路由标题, 用于显示面包屑, 页面标题 *推荐设置
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 如需外部打开，增加：_blank
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 内链打开http链接
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public bool HideMenu { get; internal set; }
    }
}