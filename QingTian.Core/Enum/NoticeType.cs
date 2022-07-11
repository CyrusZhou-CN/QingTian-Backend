using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        NOTICE = 1,

        /// <summary>
        /// 消息
        /// </summary>
        [Description("消息")]
        ANNOUNCEMENT = 2,
    }
}
