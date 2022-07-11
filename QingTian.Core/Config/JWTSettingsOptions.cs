using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// JWT配置
    /// </summary>
    public class JWTSettingsOptions : IConfigurableOptions
    {
        /// <summary>
        /// 是否验证密钥
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string IssuerSigningKey { get; set; }
        /// <summary>
        /// 是否验证签发方
        /// </summary>
        public bool ValidateIssuer { get; set; }
        /// <summary>
        /// 签发方
        /// </summary>
        public string ValidIssuer { get; set; }
        /// <summary>
        /// 是否验证签收方
        /// </summary>
        public bool ValidateAudience { get; set; }
        /// <summary>
        /// 签收方
        /// </summary>
        public string ValidAudience { get; set; }
        /// <summary>
        /// 是否验证过期时间
        /// </summary>
        public bool ValidateLifetime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpiredTime { get; set; }
        /// <summary>
        /// 过期时间容错值
        /// </summary>
        public long ClockSkew { get; set; }
        /// <summary>
        /// 加密算法
        /// </summary>
        public string Algorithm { get; set; }
}
}
