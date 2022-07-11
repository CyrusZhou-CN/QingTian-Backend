using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [SugarTable("sys_role")]
    [Description("角色表")]
    public class SysRole : DbEntityBase
    {
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
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 数据范围类型（字典 1全部数据 2本部门及以下数据 3本部门数据 4仅本人数据 5自定义数据）
        /// </summary>
        public DataScopeType DataScopeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;

    }
}