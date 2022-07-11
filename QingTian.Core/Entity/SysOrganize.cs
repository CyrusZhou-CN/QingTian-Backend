using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 组织机构表
    /// </summary>
    [SugarTable("sys_organize")]
    [Description("组织机构表")]
    public class SysOrganize : DbEntityBase
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
        [Required, MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Tel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

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