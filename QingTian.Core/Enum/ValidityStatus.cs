using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum ValidityStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        ENABLE = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        DISABLE = 1,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        DELETED = 2
    }
}
