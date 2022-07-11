namespace QingTian.Core.Services
{
    /// <summary>
    /// 附属机构和职位参数
    /// </summary>
    public class EmpPosView
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public long SysEmpId { get; set; }
        /// <summary>
        /// 职位Id
        /// </summary>
        public long PosId { get; set; }

        /// <summary>
        /// 职位编码
        /// </summary>
        public string PosCode { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PosName { get; set; }
    }
}