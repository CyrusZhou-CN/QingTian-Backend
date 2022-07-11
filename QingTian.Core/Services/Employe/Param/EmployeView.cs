namespace QingTian.Core.Services
{
    public class EmployeView
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public long Id { get; internal set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 机构与职位信息
        /// </summary>
        public List<EmpExtOrgPosView> ExtOrgPos { get; set; } = new List<EmpExtOrgPosView>();


        /// <summary>
        /// 职位信息
        /// </summary>
        public List<EmpPosView> Positions { get; set; } = new List<EmpPosView>();
    }
}