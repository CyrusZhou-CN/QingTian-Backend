using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("sys_user")]
    [Description("用户表")]
    public class SysUser : DbEntityBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required, MaxLength(20)]
        [SugarColumn(ColumnDescription = "账号", IsNullable = true)]
        [Description("账号")]
        public string Account { get; set; }
        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        [Required, MaxLength(50)]
        [SugarColumn(ColumnDescription = "密码（默认MD5加密）", IsNullable = true)]
        [Description("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "真实姓名")]
        [Description("真实姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(200)]
        [SugarColumn(ColumnDescription = "默认首页", IsNullable = true)]
        [Description("默认首页")]
        public string HomePath { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "昵称", IsNullable = true)]
        [Description("昵称")]
        public string NickName { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(ColumnDescription = "头像", IsNullable = true)]
        [Description("头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [SugarColumn(ColumnDescription = "生日", IsNullable = true)]
        [Description("生日")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别: 男=1 女=2
        /// </summary>
        [SugarColumn(ColumnDescription = "性别: 男=1 女=2")]
        [Description("性别")]
        public Gender Sex { get; set; } = Gender.UNKNOWN;

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "邮箱", IsNullable = true)]
        [Description("邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "手机", IsNullable = true)]
        [Description("手机")]
        public string Phone { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "最后登录IP", IsNullable = true)]
        [Description("最后登录IP")]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [SugarColumn(ColumnDescription = "最后登录时间", IsNullable = true)]
        [Description("最后登录时间")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 用户类型：普通用户=1 | 管理员=2 | 超级管理员=3
        /// </summary>
        [SugarColumn(ColumnDescription = "用户类型：普通用户=1 | 管理员=2 | 超级管理员=3")]
        [Description("用户类型")]
        public UserType UserType { get; set; } = UserType.User;

        /// <summary>
        /// 状态：正常=0 | 停用=1 | 删除=2
        /// </summary>
        [SugarColumn(ColumnDescription = "状态：正常=0 | 停用=1 | 删除=2")]
        [Description("状态")]
        public ValidityStatus Status { get; set; } = ValidityStatus.ENABLE;
    }
}
