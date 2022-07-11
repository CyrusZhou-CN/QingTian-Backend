namespace QingTian.Core.Services
{
    /// <summary>
    /// 菜单参数
    /// </summary>
    public class MenuParam
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public virtual long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 菜单类型（字典 0目录 1菜单 2按钮）
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public virtual string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public virtual string Permission { get; set; }

        /// <summary>
        /// 打开方式: 0=无 1=组件 2=内链 3=外链
        /// </summary>
        public virtual int OpenType { get; set; }

        /// <summary>
        /// 是否可见（0-是，1-否）
        /// </summary>
        public virtual int Visible { get; set; }

        /// <summary>
        /// 内链地址
        /// </summary>
        public virtual string Link { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public virtual string Redirect { get; set; }

        /// <summary>
        /// 权重（字典 1系统权重 2业务权重）
        /// </summary>
        public virtual string Weight { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态: 正常=0 停用=1 删除=2）
        /// </summary>
        public virtual int Status { get; set; }
    }
}