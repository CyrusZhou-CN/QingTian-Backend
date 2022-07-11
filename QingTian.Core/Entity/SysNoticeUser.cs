using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 通知消息用户表
    /// </summary>
    [SugarTable("sys_notice_user")]
    [Description("通知消息用户表")]
    public class SysNoticeUser : IEntity
    {
        /// <summary>
        /// 通知消息Id
        /// </summary>
        public long NoticeId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 状态: 0=未读 1=已读
        /// </summary>
        public NoticeUserStatus ReadStatus { get; set; }

        public void Create()
        {
        }

        public void Modify()
        {
        }
    }
}
