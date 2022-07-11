namespace QingTian.Core.Services
{
    public class UserView
    { 
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string RealName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 性别: 男=1 女=2
        /// </summary>
        public virtual int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 状态: 0=正常 1=停用 2=删除
        /// </summary>
        public virtual int Status { get; set; }

    }
}