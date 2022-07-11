using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemSettingsOptions : IConfigurableOptions
    {
        /// <summary>
        /// 是否开启全局日志
        /// </summary>
        public bool IsGlobalRequestLog { set; get; }
        /// <summary>
        /// 单位是字节（byte） 1kb=1024byte,此处限制40M
        /// </summary>
        public long MaxRequestBodySize { set; get; }
    }
}
