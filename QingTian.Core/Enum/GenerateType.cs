using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 代码生成方式
    /// </summary>
    public enum GenerateType
    {
        /// <summary>
        /// 项目目录
        /// </summary>
        [Description("项目目录")]
        ProjectDirectory = 0,
        /// <summary>
        /// 下载项目
        /// </summary>
        [Description("下载项目")]
        Download = 1
    }
}
