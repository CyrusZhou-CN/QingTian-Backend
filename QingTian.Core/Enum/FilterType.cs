using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 0,

        /// <summary>
        /// 组织
        /// </summary>
        [Description("组织")]
        Org = 1,
    }
}
