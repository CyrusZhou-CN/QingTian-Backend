using System;
using QingTian.Core;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 访问日志表输出参数
    /// </summary>
    public class SysLogVisPut
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 访问人
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public long Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public long VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisTime { get; set; }

        /// <summary>
        /// 访问账号
        /// </summary>
        public string Account { get; set; }

    }
}
