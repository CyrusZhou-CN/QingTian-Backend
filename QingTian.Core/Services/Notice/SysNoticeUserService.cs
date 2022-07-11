using Furion.DependencyInjection;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services.Notice
{
    /// <inheritdoc cref="ISysNoticeUserService"/>
    public class SysNoticeUserService : ISysNoticeUserService, ITransient
    {
        private readonly SqlSugarRepository<SysNoticeUser> _sysNoticeUserRep;  // 通知消息用户表 

        public SysNoticeUserService(SqlSugarRepository<SysNoticeUser> sysNoticeUserRep)
        {
            _sysNoticeUserRep = sysNoticeUserRep;
        }

        /// <inheritdoc/>
        public async Task AddAsync(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus)
        {
            List<SysNoticeUser> list = new List<SysNoticeUser>();
            noticeUserIdList.ForEach(u =>
            {
                list.Add(new SysNoticeUser
                {
                    NoticeId = noticeId,
                    UserId = u,
                    ReadStatus = noticeUserStatus
                });
            });
            await _sysNoticeUserRep.InsertAsync(list);
        }

        /// <inheritdoc/>
        public async Task<List<SysNoticeUser>> GetNoticeUserListByNoticeIdAsync(long noticeId)
        {
            return await _sysNoticeUserRep.Where(u => u.NoticeId == noticeId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SetReadStatusAsync(long noticeId, long userId, NoticeUserStatus status)
        {
            await _sysNoticeUserRep.UpdateAsync(m => m.NoticeId == noticeId && m.UserId == userId, m => new SysNoticeUser
            {
                ReadStatus = status,
                ReadTime = DateTime.Now
            });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus)
        {
            await _sysNoticeUserRep.DeleteAsync(u => u.NoticeId == noticeId);

            await AddAsync(noticeId, noticeUserIdList, noticeUserStatus);
        }
    }
}
