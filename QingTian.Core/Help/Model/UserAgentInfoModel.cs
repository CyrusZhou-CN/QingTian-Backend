using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// UserAgent 信息Model类
    /// </summary>
    public class UserAgentInfoModel
    {
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }

        /// <summary>
        /// 操作系统（版本）
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 浏览器（版本）
        /// </summary>
        public string Browser { get; set; }
    }
}
