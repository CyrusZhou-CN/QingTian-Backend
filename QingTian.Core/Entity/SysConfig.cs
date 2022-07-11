using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 参数配置表
    /// </summary>
    [SugarTable("sys_config")]
    [Description("参数配置表")]
    public class SysConfig : DbEntityBase
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
        /// 属性值
        /// </summary>
        [MaxLength(50)]
        public string Value { get; set; }

        /// <summary>
        /// 是否是系统参数（1-是，0-否）
        /// </summary>
        public YesOrNo SysFlag { get; set; } = YesOrNo.Yes;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;

        /// <summary>
        /// 常量所属分类的编码，来自于“常量的分类”字典
        /// </summary>
        [MaxLength(50)]
        public string GroupCode { get; set; }
    }
}