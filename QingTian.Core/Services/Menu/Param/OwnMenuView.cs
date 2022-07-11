namespace QingTian.Core.Services
{
    public class OwnMenuView
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long SysRoleId { get; set; }

        /// <summary>
        /// 菜单Id集合
        /// </summary>
        public List<long> MenuIdList { get; set; }
    }
}