namespace QingTian.Core.Services
{
    public class EmployeView2
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 员工工号
        /// </summary>
        public string JobNum { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 附属机构
        /// </summary>
        public List<EmpExtOrgPosView> ExtIds { get; set; } = new List<EmpExtOrgPosView>();

        /// <summary>
        /// 职位集合
        /// </summary>
        public List<long> PosIdList { get; set; } = new List<long>();
    }
}