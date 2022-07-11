using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [SugarTable("sys_dict_type")]
    [Description("字典类型表")]
    public class SysDictType : DbEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required, MaxLength(50)]
        public string Code { get; set; }

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
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;
    }
}