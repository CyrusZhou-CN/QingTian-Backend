using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class ConnectionStringsOptions : IConfigurableOptions
    {
        /// <summary>
        /// 默认数据库编号
        /// </summary>
        public string DefaultDbNumber { get; set; }
        /// <summary>
        /// 默认数据库类型
        /// </summary>
        public string DefaultDbType { get; set; }
        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>

        public string DefaultDbString { get; set; }
        /// <summary>
        /// 业务库集合
        /// </summary>
        public List<DbConfig> DbConfigs { get; set; }
    }
}
