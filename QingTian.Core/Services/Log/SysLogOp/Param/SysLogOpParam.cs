using QingTian.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 操作日志表输入参数
    /// </summary>
    public class SysLogOpParam : PageParamBase
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public virtual long? Success { get; set; }

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
        /// 请求地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public virtual string MethodName { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public virtual string ReqMethod { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public virtual string Param { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public virtual string Result { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public virtual long? ElapsedTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime? OpTime { get; set; }

        public virtual DateTime?[]? OpTimes { get; set; }

        /// <summary>
        /// 操作账号
        /// </summary>
        public virtual string Account { get; set; }

    }

    public class AddSysLogOpParam : SysLogOpParam
    {
    }

    public class DeleteSysLogOpParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class UpdateSysLogOpParam : SysLogOpParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class QueryeSysLogOpParam : DeleteSysLogOpParam
    {

    }
}
