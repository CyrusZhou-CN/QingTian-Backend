using QingTian.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 异常日志输入参数
    /// </summary>
    public class SysLogExParam : PageParamBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public virtual string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public virtual string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public virtual string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public virtual string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        public virtual string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        public virtual string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public virtual DateTime ExceptionTime { get; set; }

    }

    public class AddSysLogExParam : SysLogExParam
    {
    }

    public class DeleteSysLogExParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class UpdateSysLogExParam : SysLogExParam
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

    }

    public class QueryeSysLogExParam : DeleteSysLogExParam
    {

    }
}
