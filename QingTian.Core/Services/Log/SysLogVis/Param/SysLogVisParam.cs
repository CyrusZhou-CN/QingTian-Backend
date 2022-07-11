using QingTian.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 访问日志表输入参数
    /// </summary>
    public class SysLogVisParam : PageParamBase
    {
        /// <summary>
        /// 访问人
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public virtual long Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public virtual string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public virtual string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public virtual string Os { get; set; }

        /// <summary>
        /// 访问类型 登陆 = 0, 登出 = 1, 注册 = 2, 改密 = 3,第三方登陆 = 4, 权限信息 = 5
        /// </summary>
        public virtual long VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public virtual DateTime VisTime { get; set; }

        /// <summary>
        /// 访问账号
        /// </summary>
        public virtual string Account { get; set; }

    }

    public class AddSysLogVisParam : SysLogVisParam
    {
    }

    public class DeleteSysLogVisParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class UpdateSysLogVisParam : SysLogVisParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class QueryeSysLogVisParam : DeleteSysLogVisParam
    {

    }
}
