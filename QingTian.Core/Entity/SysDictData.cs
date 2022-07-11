using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 字典值表
    /// </summary>
    [SugarTable("sys_dict_data")]
    [Description("字典值表")]
    public class SysDictData : DbEntityBase
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [MaxLength(100)]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
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