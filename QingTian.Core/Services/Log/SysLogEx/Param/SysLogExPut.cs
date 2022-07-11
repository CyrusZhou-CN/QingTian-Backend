using System;
using QingTian.Core;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 异常日志输出参数
    /// </summary>
    public class SysLogExPut
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        public string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTime ExceptionTime { get; set; }

    }
}
