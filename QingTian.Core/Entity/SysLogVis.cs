using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 访问日志表
    /// </summary>
    [SugarTable("sys_log_vis")]
    [Description("访问日志表")]
    public class SysLogVis : AutoIncrementEntity
    {

        /// <summary>
        /// 访问人
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Account { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// 是否执行成功（1-是，0-否）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public YesOrNo Success { get; set; }

        /// <summary>
        /// 具体消息
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(IsNullable = true)]
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(IsNullable = true)]
        public string Os { get; set; }

        /// <summary>
        /// 访问类型 登陆 = 0, 登出 = 1, 注册 = 2, 改密 = 3,第三方登陆 = 4, 权限信息 = 5
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public LoginType VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? VisTime { get; set; }
    }
}
