using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 操作日志表
    /// </summary>
    [SugarTable("sys_log_op")]
    [Description("操作日志表")]
    public class SysLogOp : AutoIncrementEntity
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
        /// 是否执行成功（1-是，0-否）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public YesOrNo Success { get; set; }

        /// <summary>
        /// 具体消息
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        [SugarColumn(IsNullable = true)]
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Os { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Url { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string MethodName { get; set; }

        /// <summary>
        /// 请求方式（GET POST PUT DELETE)
        /// </summary>
        [MaxLength(10)]
        [SugarColumn(IsNullable = true)]
        public string ReqMethod { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Param { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Result { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime OpTime { get; set; }
    }
}