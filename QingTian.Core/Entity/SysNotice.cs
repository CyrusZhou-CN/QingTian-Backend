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
    /// 通知消息表
    /// </summary>
    [SugarTable("sys_notice")]
    [Description("通知消息表")]
    public class SysNotice : DbEntityBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required, MaxLength(20)]
        [SugarColumn(ColumnDescription = "标题")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [SugarColumn(ColumnDescription = "内容")]
        public string Content { get; set; }

        /// <summary>
        /// 类型: 1=通知 2=消息
        /// </summary>
        [SugarColumn(ColumnDescription = "类型")]
        public NoticeType Type { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        [SugarColumn(ColumnDescription = "发布人Id")]
        public long PublicUserId { get; set; }

        /// <summary>
        /// 发布人姓名
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "发布人姓名")]
        public string PublicUserName { get; set; }

        /// <summary>
        /// 发布机构Id
        /// </summary>
        [SugarColumn(ColumnDescription = "发布机构Id", IsNullable = true)]
        public long PublicOrgId { get; set; }

        /// <summary>
        /// 发布机构名称
        /// </summary>
        [MaxLength(50)]
        [SugarColumn(ColumnDescription = "发布机构名称", IsNullable = true)]
        public string PublicOrgName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [SugarColumn(ColumnDescription = "发布时间")]
        public DateTime PublicTime { get; set; }

        /// <summary>
        /// 撤回时间
        /// </summary>
        [SugarColumn(ColumnDescription = "撤回时间")]
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 状态: 0=草稿 1=发布 2=撤回 3=删除
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public NoticeStatus Status { get; set; }
        
    }
}
