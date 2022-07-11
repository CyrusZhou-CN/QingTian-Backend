using Furion;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{

    /// <inheritdoc cref="ISysOrganizeService"/>
    [Route("SysOrganize"), ApiDescriptionSettings(Name = "Organize", Order = 997)]
    public class SysOrganizeService : ISysOrganizeService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysOrganize> _sysOrgRep;  // 组织机构表 
        private readonly SqlSugarRepository<SysUserDataScope> _sysUserDataScopeRep;
        private readonly SqlSugarRepository<SysRoleDataScope> _sysRoleDataScopeRep;
        private readonly ISysCacheService _sysCacheService;
        private readonly ISysEmpService _sysEmpService;
        private readonly ISysEmpExtOrgPosService _sysEmpExtOrgPosService;

        public SysOrganizeService(SqlSugarRepository<SysOrganize> sysOrgRep,
                             SqlSugarRepository<SysUserDataScope> sysUserDataScopeRep,
                             SqlSugarRepository<SysRoleDataScope> sysRoleDataScopeRep,
                             ISysCacheService sysCacheService,
                             ISysEmpService sysEmpService,
                             ISysEmpExtOrgPosService sysEmpExtOrgPosService)
        {
            _sysOrgRep = sysOrgRep;
            _sysUserDataScopeRep = sysUserDataScopeRep;
            _sysRoleDataScopeRep = sysRoleDataScopeRep;
            _sysCacheService = sysCacheService;
            _sysEmpService = sysEmpService;
            _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
        }


        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddOrganize(AddOrganizeParam param)
        {
            var isExist = await _sysOrgRep.AnyAsync(u => u.Name == param.Name && u.Pid.ToString() == param.Pid);
            if (isExist)
                throw Oops.Oh(ErrorCode.E2002);
            if (!AppUser.IsSuperAdmin)
            {
                // 如果新增的机构父Id不是0，则进行数据权限校验
                if (param.Pid != "0" && !string.IsNullOrEmpty(param.Pid))
                {
                    // 新增组织机构的父机构不在自己的数据范围内
                    long.Parse(param.Pid).CheckDataScope(FilterType.Org);
                }
                else
                    throw Oops.Oh(ErrorCode.E2006);
            }

            var sysOrg = param.Adapt<SysOrganize>();
            await FillPids(sysOrg);
            var newOrg = await _sysOrgRep.InsertReturnEntityAsync(sysOrg);

            //// 测试写入大量数据
            //List<SysOrganize> sysOrgs = new List<SysOrganize>();
            //var name = sysOrg.Name;
            //for (int i = 0; i < 100000; i++)
            //{
            //    sysOrg.Name = $"{name}{i}";
            //    sysOrg.Id = 0;
            //    var _sysOrg = sysOrg.Adapt<SysOrganize>();
            //    _sysOrg.Create();
            //    sysOrgs.Add(_sysOrg);
            //}
            //await _sysOrgRep.Context.Fastest<SysOrganize>().BulkCopyAsync(sysOrgs);

            // 当前用户不是超级管理员时，将新增的公司加到用户的数据权限
            if (App.User.FindFirst(ConstClaim.SuperAdmin)?.Value != ((int)UserType.SuperAdmin).ToString())
            {
                var userId = App.User.FindFirst(ConstClaim.UserId)?.Value;
                await _sysUserDataScopeRep.InsertAsync(new SysUserDataScope
                {
                    SysUserId = long.Parse(userId),
                    SysOrgId = newOrg.Id
                });
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);
                await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
            }
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeleteOrganize(DeleteOrganizeParam param)
        {
            var sysOrg = await _sysOrgRep.FirstOrDefaultAsync(u => u.Id == long.Parse(param.Id));

            // 检测数据范围能不能操作这个机构
            long.Parse(param.Id).CheckDataScope(FilterType.Org);

            // 该机构下有员工，则不能删
            var hasOrgEmp = await _sysEmpService.HasOrgEmp(sysOrg.Id);
            if (hasOrgEmp)
                throw Oops.Oh(ErrorCode.E2004);

            // 该机构下面子机构若有员工，则不能删
            var orgIds = await _sysOrgRep.Where(u => u.Pids.Contains(param.Id)).Select(u => u.Id).ToListAsync();
            var emps = await _sysEmpService.HasOrgEmp(orgIds);
            if (emps.Count > 0)
                throw Oops.Oh(ErrorCode.E2004);

            // 该附属机构下若有员工，则不能删
            var hasExtOrgEmp = await _sysEmpExtOrgPosService.HasExtOrgEmp(sysOrg.Id);
            if (hasExtOrgEmp)
                throw Oops.Oh(ErrorCode.E2005);

            // 级联删除子节点
            var childIdList = await GetChildIdListWithSelfById(sysOrg.Id);

            try
            {
                _sysOrgRep.Ado.BeginTran();


                // 级联删除该机构及子机构对应的角色-数据范围关联信息
                await _sysRoleDataScopeRep.DeleteAsync(u => childIdList.Contains(u.SysOrgId));

                // 级联删除该机构子机构对应的用户-数据范围关联信息
                await _sysUserDataScopeRep.DeleteAsync(u => childIdList.Contains(u.SysOrgId));

                await _sysOrgRep.DeleteAsync(u => childIdList.Contains(u.Id));
                _sysOrgRep.Ado.CommitTran();

            }
            catch (System.Exception)
            {
                _sysOrgRep.Ado.RollbackTran();
                throw;
            }
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
        }


        /// <inheritdoc/>
        [NonAction]
        public async Task<List<long>> GetAllDataScopeIdList()
        {
            return await _sysOrgRep.AsQueryable().Select(u => u.Id).ToListAsync();
        }


        /// <inheritdoc/>
        [NonAction]
        public async Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId)
        {
            var orgIdList = new List<long>();
            if (orgId == 0)
                return orgIdList;

            // 如果是范围类型是全部数据，则获取当前所有的组织架构Id
            if (dataScopeType == (int)DataScopeType.ALL)
            {
                orgIdList = await _sysOrgRep.Where(u => u.Status == (int)ValidityStatus.ENABLE).Select(u => u.Id).ToListAsync();
            }
            // 如果范围类型是本部门及以下部门，则查询本节点和子节点集合，包含本节点
            else if (dataScopeType == (int)DataScopeType.DEPT_WITH_CHILD)
            {
                orgIdList = await GetChildIdListWithSelfById(orgId);
            }
            // 如果数据范围是本部门，不含子节点，则直接返回本部门
            else if (dataScopeType == (int)DataScopeType.DEPT)
            {
                orgIdList.Add(orgId);
            }
            return orgIdList;
        }


        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysOrganize> GetOrganize([FromQuery] QueryOrganizeParam param)
        {
            return await _sysOrgRep.FirstOrDefaultAsync(u => u.Id == long.Parse(param.Id));
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<List<OrganizeView>> GetOrganizeList([FromQuery] OrganizeParam param)
        {
            var dataScopeList = GetDataScopeList(await DataFilterExtensions.GetDataScopeIdList(FilterType.Org));
            var orgs = await _sysOrgRep.AsQueryable()
                                       .WhereIF(!string.IsNullOrWhiteSpace(param.Pid), u => u.Pid == long.Parse(param.Pid))
                                       .Where(u => u.Status != ValidityStatus.DELETED)
                                       .WhereIF(dataScopeList.Any(), u => dataScopeList.Contains(u.Id)) // 非管理员范围限制
                                       .OrderBy(u => u.Sort)
                                       .ToListAsync();
            return orgs.Adapt<List<OrganizeView>>();
        }

        /// <inheritdoc/>
        [HttpGet("tree")]
        public async Task<dynamic> GetOrganizeTree([FromQuery] OrganizeParam Param)
        {
            var dataScopeList = new List<long>();
            if (!AppUser.IsSuperAdmin)
            {
                var dataScopes = await DataFilterExtensions.GetDataScopeIdList(FilterType.Org);
                if (dataScopes.Count < 1)
                    return dataScopeList;
                dataScopeList = GetDataScopeList(dataScopes);
            }
            var orgs = await _sysOrgRep.Where(dataScopeList.Any(), u => dataScopeList.Contains(u.Id))
                                                        .Where(u => u.Status == (int)ValidityStatus.ENABLE).OrderBy(u => u.Sort)
                                                        .Select(u => new OrganizeTreeNode
                                                        {
                                                            Id = u.Id,
                                                            Pid = u.Pid,
                                                            Name = u.Name,
                                                            Sort = u.Sort,
                                                            CreatedTime = u.CreatedTime,
                                                            Remark = u.Remark,
                                                        }).ToListAsync();

            return new TreeBuildUtil<OrganizeTreeNode>().DoTreeBuild(orgs);
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryOrganizePageList([FromQuery] PageOrganizeParam param)
        {
            var dataScopeList = GetDataScopeList(await DataFilterExtensions.GetDataScopeIdList(FilterType.Org));
            var orgs = await _sysOrgRep.AsQueryable()
                                       .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                                       .WhereIF(!string.IsNullOrWhiteSpace(param.Id), u => u.Id == long.Parse(param.Id.Trim()))
                                       .WhereIF(!string.IsNullOrWhiteSpace(param.Pid), u => u.Pids.Contains(param.Pid.Trim()) || u.Id == long.Parse(param.Pid.Trim()))
                                       .WhereIF(dataScopeList.Any(), u => dataScopeList.Contains(u.Id)) // 非管理员范围限制
                                       .Where(m => m.Pid == 0)
                                       .OrderBy(u => u.Sort)
                                       .Select(u => new OrganizeTreeNode
                                       {
                                           Id = u.Id,
                                           Pid = u.Pid,
                                           Name = u.Name,
                                           Sort = u.Sort,
                                           CreatedTime = u.CreatedTime,
                                           Status = u.Status,
                                           Remark = u.Remark
                                       })
                                       .ToPagedListAsync(param.Page, param.PageSize);
            List<OrganizeTreeNode> childrens = new List<OrganizeTreeNode>();
            childrens.AddRange(orgs.Items);
            foreach (var item in orgs.Items)
            {
                var child = await _sysOrgRep.AsQueryable()
                    .Where(m => m.Pids.Contains(item.Id.ToString()))
                    .Select(u => new OrganizeTreeNode
                    {
                        Id = u.Id,
                        Pid = u.Pid,
                        Name = u.Name,
                        Sort = u.Sort,
                        CreatedTime = u.CreatedTime,
                        Status = u.Status,
                        Remark = u.Remark
                    }).ToListAsync();
                childrens.AddRange(child);
            }
            orgs.Items = new TreeBuildUtil<OrganizeTreeNode>().DoTreeBuild(childrens);
            return orgs.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateOrganize(UpdateOrganizeParam param)
        {
            if (param.Pid != "0" && !string.IsNullOrEmpty(param.Pid))
            {
                var org = await _sysOrgRep.FirstOrDefaultAsync(u => u.Id == long.Parse(param.Pid));
                _ = org ?? throw Oops.Oh(ErrorCode.E2000);
            }
            if (param.Id == param.Pid)
                throw Oops.Oh(ErrorCode.E2001);

            // 如果是编辑，父id不能为自己的子节点
            var childIdListById = await GetChildIdListWithSelfById(long.Parse(param.Id));
            if (childIdListById.Contains(long.Parse(param.Pid)))
                throw Oops.Oh(ErrorCode.E2001);

            var sysOrg = await _sysOrgRep.FirstOrDefaultAsync(u => u.Id == long.Parse(param.Id));

            // 检测数据范围能不能操作这个机构
            sysOrg.Id.CheckDataScope(FilterType.Org);

            var isExist = await _sysOrgRep.AnyAsync(u => (u.Name == param.Name && u.Pid.ToString() == param.Pid) && u.Id != sysOrg.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.E2002);

            try
            {
                _sysOrgRep.Ado.BeginTran();
                // 如果名称有变化，则修改对应员工的机构相关信息
                if (!sysOrg.Name.Equals(param.Name))
                    await _sysEmpService.UpdateEmpOrgInfo(sysOrg.Id, param.Name);

                sysOrg = param.Adapt<SysOrganize>();
                await FillPids(sysOrg);
                await _sysOrgRep.AsUpdateable(sysOrg).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
                _sysOrgRep.Ado.CommitTran();

            }
            catch (System.Exception)
            {
                _sysOrgRep.Ado.RollbackTran();
                throw;
            }
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_DATASCOPE);
            await _sysCacheService.DelByStartsWithAsync(ConstCache.CACHE_KEY_USERSDATASCOPE);
        }

        /// <inheritdoc/>
        private async Task FillPids(SysOrganize sysOrg)
        {
            if (sysOrg.Pid == 0)
            {
                sysOrg.Pids = $"[0],";
            }
            else
            {
                var t = await _sysOrgRep.FirstOrDefaultAsync(u => u.Id == sysOrg.Pid);
                sysOrg.Pids = t.Pids + "[" + t.Id + "],";
            }
        }

        /// <inheritdoc/>
        private async Task<List<long>> GetChildIdListWithSelfById(long id)
        {
            var childIdList = await _sysOrgRep
                .Where(u => u.Pids.Contains(id.ToString()))
                .Select(u => u.Id)
                .ToListAsync();
            childIdList.Add(id);
            return childIdList;
        }

        /// <inheritdoc/>
        private List<long> GetDataScopeList(List<long> dataScopes)
        {
            var dataScopeList = new List<long>();
            // 如果是超级管理员则获取所有组织机构，否则只获取其数据范围的机构数据
            if (!AppUser.IsSuperAdmin)
            {
                if (dataScopes.Count < 1)
                    return dataScopeList;

                // 此处获取所有的上级节点，用于构造完整树
                dataScopes.ForEach(u =>
                {
                    var sysOrg = _sysOrgRep.FirstOrDefault(c => c.Id == u);
                    var parentAndChildIdListWithSelf = sysOrg.Pids.TrimEnd(',').Replace("[", "").Replace("]", "")
                                                                  .Split(",").Select(u => long.Parse(u)).ToList();
                    parentAndChildIdListWithSelf.Add(sysOrg.Id);
                    dataScopeList.AddRange(parentAndChildIdListWithSelf);
                });
            }

            return dataScopeList;
        }
    }
}
