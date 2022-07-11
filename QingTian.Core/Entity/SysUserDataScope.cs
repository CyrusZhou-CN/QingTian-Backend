using SqlSugar;
using System.ComponentModel;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 用户数据范围表
    /// </summary>
    [SugarTable("sys_user_data_scope")]
    [Description("用户数据范围表")]
    public class SysUserDataScope : IEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long SysUserId { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long SysOrgId { get; set; }

        public void Create()
        {
        }

        public void Modify()
        {
        }
    }
}