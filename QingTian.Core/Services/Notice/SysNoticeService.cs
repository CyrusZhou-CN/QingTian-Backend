using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysNoticeService"/>
    [Route("SysNotice"), ApiDescriptionSettings(Name = "Notice", Order = 100)]
    public class SysNoticeService : ISysNoticeService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysNotice> _sysNoticeRep;  // 通知消息表
        private readonly ISysOnlineUserService _sysOnlineUserService;

        private readonly ISysNoticeUserService _sysNoticeUserService;
        private readonly SqlSugarRepository<SysEmploye> _sysEmpRep;

        public SysNoticeService(SqlSugarRepository<SysNotice> sysNoticeRep,
                                ISysNoticeUserService sysNoticeUserService, ISysOnlineUserService sysOnlineUserService, SqlSugarRepository<SysEmploye> sysEmpRep)
        {
            _sysNoticeRep = sysNoticeRep;
            _sysNoticeUserService = sysNoticeUserService;
            _sysOnlineUserService = sysOnlineUserService;
            _sysEmpRep = sysEmpRep;
        }
        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddNotice(AddNoticeParam param)
        {
            if (param.Status != NoticeStatus.DRAFT && param.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.E7000);

            var notice = param.Adapt<SysNotice>();
            await UpdatePublicInfo(notice);
            // 如果是发布，则设置发布时间
            if (param.Status == NoticeStatus.PUBLIC)
                notice.PublicTime = DateTime.Now;
            var newItem = await _sysNoticeRep.InsertReturnEntityAsync(notice);

            // 通知到的人
            var noticeUserIdList = param.NoticeUserIdList;
            var noticeUserStatus = NoticeUserStatus.UNREAD;
            await _sysNoticeUserService.AddAsync(newItem.Id, noticeUserIdList, noticeUserStatus);
            if (newItem.Status == NoticeStatus.PUBLIC)
            {
                await _sysOnlineUserService.PushNotice(newItem, noticeUserIdList);
            }
        }

        /// <inheritdoc/>
        [HttpPost("changeStatus")]
        public async Task ChangeStatus(ChangeStatusNoticeParam param)
        {

            // 状态应为撤回或删除或发布
            if (param.Status != NoticeStatus.CANCEL && param.Status != NoticeStatus.DELETED && param.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.E7000);

            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            notice.Status = param.Status;

            if (param.Status == NoticeStatus.CANCEL)
            {
                notice.CancelTime = DateTime.Now;
            }
            else if (param.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTime.Now;
            }
            await _sysNoticeRep.UpdateAsync(notice);
            if (notice.Status == NoticeStatus.PUBLIC)
            {
                // 获取通知到的用户
                var noticeUserList = await _sysNoticeUserService.GetNoticeUserListByNoticeIdAsync(param.Id);
                await _sysOnlineUserService.PushNotice(notice, noticeUserList.Select(m => m.UserId).ToList());
            }
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteNotice(DeleteNoticeParam param)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            if (notice.Status != NoticeStatus.DRAFT && notice.Status != NoticeStatus.CANCEL) // 只能删除草稿和撤回的
                throw Oops.Oh(ErrorCode.E7001);

            await _sysNoticeRep.DeleteAsync(notice);
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<NoticeDetailView> GetNotice([FromQuery] QueryNoticeParam param)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == param.Id);

            // 获取通知到的用户
            var noticeUserList = await _sysNoticeUserService.GetNoticeUserListByNoticeIdAsync(param.Id);
            var noticeUserIdList = new List<string>();
            var noticeUserReadInfoList = new List<NoticeUserRead>();
            if (noticeUserList != null)
            {
                noticeUserList.ForEach(u =>
                {
                    noticeUserIdList.Add(u.UserId.ToString());
                    var noticeUserRead = new NoticeUserRead
                    {
                        UserId = u.UserId,
                        UserName = AppUser.UserName,
                        ReadStatus = u.ReadStatus,
                        ReadTime = u.ReadTime
                    };
                    noticeUserReadInfoList.Add(noticeUserRead);
                });
            }
            var noticeResult = notice.Adapt<NoticeDetailView>();
            noticeResult.NoticeUserIdList = noticeUserIdList;
            noticeResult.NoticeUserReadInfoList = noticeUserReadInfoList;
            // 如果该条通知消息为已发布，则将当前用户的该条通知消息设置为已读
            if (notice.Status == NoticeStatus.PUBLIC)
                await _sysNoticeUserService.SetReadStatusAsync(notice.Id, AppUser.UserId, NoticeUserStatus.READ);
            return noticeResult;
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryNoticePageList([FromQuery] NoticeParam param)
        {
            var notices = await _sysNoticeRep.AsQueryable()
                                            .WhereIF(!string.IsNullOrWhiteSpace(param.SearchValue), u => u.Title.Contains(param.SearchValue.Trim()) || u.Content.Contains(param.SearchValue.Trim()))
                                            .WhereIF(param.Type > 0, u => u.Type == param.Type)
                                            .Where(u => u.Status != NoticeStatus.DELETED)
                                            .ToPagedListAsync(param.Page, param.PageSize);
            return notices.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpGet("received")]
        public async Task<dynamic> ReceivedNoticePageList([FromQuery] NoticeParam param)
        {
            var notices = await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId))
             .Where((n, u) => u.UserId == AppUser.UserId)
             .WhereIF(!string.IsNullOrWhiteSpace(param.SearchValue), n => n.Title.Contains(param.SearchValue.Trim()) || n.Content.Contains(param.SearchValue.Trim()))
             .WhereIF(param.Type > 0, (n, u) => n.Type == param.Type)
             .Select<NoticeReceiveView>()
             .ToPagedListAsync(param.Page, param.PageSize);
            return notices.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpGet("unread")]
        public async Task<dynamic> UnReadNoticeList([FromQuery] NoticeParam param)
        {
            var dic = typeof(NoticeType).EnumToList();
            var notices = await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId))
             .Where((n, u) => u.UserId == AppUser.UserId && u.ReadStatus == NoticeUserStatus.UNREAD).PartitionBy(n => n.Type).OrderBy(n => n.CreatedTime, OrderByType.Desc).Take(param.PageSize).Select<NoticeReceiveView>()
             .ToListAsync();
            var count = await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId)).Where((n, u) => u.UserId == AppUser.UserId && u.ReadStatus == NoticeUserStatus.UNREAD).CountAsync();

            List<dynamic> noticeClays = new List<dynamic>();
            int index = 0;
            foreach (var item in dic)
            {
                noticeClays.Add(
                    new
                    {
                        Index = index++,
                        Key = item.Describe,
                        Value = item.Value,
                        NoticeData = notices.Where(m => m.Type == item.Value).ToList()
                    }
                );
            }
            return new
            {
                Items = noticeClays,
                Total = count
            };
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateNotice(UpdateNoticeParam param)
        {
            if (param.Status != NoticeStatus.DRAFT && param.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.E7000);

            //  非草稿状态
            if (param.Status != NoticeStatus.DRAFT)
                throw Oops.Oh(ErrorCode.E7002);
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            notice = param.Adapt(notice);
            if (param.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTime.Now;
                await UpdatePublicInfo(notice);
            }
            await _sysNoticeRep.UpdateAsync(notice);

            // 通知到的人
            var noticeUserIdList = param.NoticeUserIdList;
            var noticeUserStatus = NoticeUserStatus.UNREAD;
            await _sysNoticeUserService.UpdateAsync(param.Id, noticeUserIdList, noticeUserStatus);
            if (notice.Status == NoticeStatus.PUBLIC)
            {
                await _sysOnlineUserService.PushNotice(notice, noticeUserIdList);
            }
        }
        /// <summary>
        /// 更新发布信息
        /// </summary>
        /// <param name="notice"></param>
        [NonAction]
        private async Task UpdatePublicInfo(SysNotice notice)
        {
            var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == AppUser.UserId);
            notice.PublicUserId = AppUser.UserId;
            notice.PublicUserName = AppUser.UserName;
            if (!emp.IsNullOrZero())
            {
                notice.PublicOrgId = emp.OrgId;
                notice.PublicOrgName = emp.OrgName; 
            }
        }
    }
}
