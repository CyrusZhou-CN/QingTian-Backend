using SqlSugar;
using System.ComponentModel;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 员工职位表
    /// </summary>
    [SugarTable("sys_emp_pos")]
    [Description("员工职位表")]
    public class SysEmpPos : IEntity
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public long SysEmpId { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long SysPosId { get; set; }

        public void Create()
        {
        }

        public void Modify()
        {
        }
    }
}