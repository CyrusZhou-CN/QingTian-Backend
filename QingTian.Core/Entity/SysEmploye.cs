using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 员工表
    /// </summary>
    [SugarTable("sys_employe")]
    [Description("员工表")]
    public class SysEmploye : DbEntityBase
    {
        /// <summary>
        /// 工号
        /// </summary>
        [MaxLength(30)]
        public string JobNum { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [MaxLength(50)]
        public string OrgName { get; set; }
    }
}