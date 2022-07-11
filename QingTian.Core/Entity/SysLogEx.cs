using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 异常日志
    /// </summary>
    [SugarTable("sys_log_ex")]
    [Description("异常日志")]
    public class SysLogEx : AutoIncrementEntity
    {
        /// <summary>
        /// 操作人
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime ExceptionTime { get; set; }
    }
}