using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 万网Ip信息Model类
    /// </summary>
    public class WhoisIPInfoModel
    {
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Pro { get; set; }

        /// <summary>
        /// 省份邮政编码
        /// </summary>
        public string ProCode { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 城市邮政编码
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 地理信息
        /// </summary>
        [JsonPropertyName("addr")]
        public string Address { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public string Operator => Address[(Pro.Length + City.Length)..].Trim();
    }
}
