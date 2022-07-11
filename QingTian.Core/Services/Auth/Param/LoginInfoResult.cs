using Furion.DependencyInjection;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 用户登录输出参数
    /// </summary>
    [SuppressSniffer]
    public class LoginInfoResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 默认首页
        /// </summary>
        public string HomePath { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别(字典 1男 2女)
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String Tel { get; set; }

        /// <summary>
        /// 管理员类型（0超级管理员 1非管理员）
        /// </summary>
        public int AdminType { get; set; }

        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登陆地址
        /// </summary>
        public string LastLoginAddress { get; set; }

        /// <summary>
        /// 最后登陆所用浏览器
        /// </summary>
        public string LastLoginBrowser { get; set; }

        /// <summary>
        /// 最后登陆所用系统
        /// </summary>
        public string LastLoginOs { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmployeView LoginEmpInfo { get; set; } = new EmployeView();

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleView> Roles { get; set; } = new List<RoleView>();

        /// <summary>
        /// 数据范围（机构）信息
        /// </summary>
        public List<long> DataScopes { get; set; } = new List<long>();
    }
}