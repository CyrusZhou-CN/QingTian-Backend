using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("sys_menu")]
    [Description("菜单表")]
    public class SysMenu : DbEntityBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 父Ids
        /// </summary>
        public string Pids { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required, MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 菜单类型: 0=目录 1=菜单 2=按钮
        /// </summary>
        public MenuType Type { get; set; } = MenuType.DIR;

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(50)]
        [SugarColumn(IsNullable = true)]
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Path { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Permission { get; set; }

        /// <summary>
        /// 打开方式: 0=无 1=组件 2=内链 3=外链
        /// </summary>
        public MenuOpenType OpenType { get; set; } = MenuOpenType.NONE;

        /// <summary>
        /// 是否可见: 0=是 1=否
        /// </summary>
        public YesOrNo Visible { get; set; } = YesOrNo.Yes;

        /// <summary>
        /// 内链地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Link { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Redirect { get; set; }

        /// <summary>
        /// 权重: 系统权重=1 业务权重=2
        /// </summary>
        public int Weight { get; set; } = 2;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 100;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 系统菜单 防止删除
        /// </summary>
        public YesOrNo SysFlag { get; set; } 

        /// <summary>
        /// 状态: 正常=0 停用=1 删除=2）
        /// </summary>
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;
    }
}