namespace QingTian.Core.Services
{
    /// <summary>
    /// 用户参数
    /// </summary>
    public class UserParam: ParamBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别: 男=1 女=2
        /// </summary>
        public Gender Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户类型：普通用户=1 | 管理员=2 | 超级管理员=3
        /// </summary>
        public UserType? UserType { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;

        /// <summary>
        /// 搜索状态: 0=正常 1=停用 2=删除
        /// </summary>
        public ValidityStatus SearchStatus { get; set; } = ValidityStatus.ENABLE;
        /// <summary>
        /// 员工信息
        /// </summary>
        public EmployeView2 SysEmpParam { get; set; } = new EmployeView2();
    }
}