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

namespace QingTian.Core.Services.Position
{

    /// <inheritdoc cref="ISysPositionService"/>
    [Route("SysPosition"), ApiDescriptionSettings(Name = "Position", Order = 996)]
    public class SysPositionService : ISysPositionService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysPosition> _sysPosRep;  // 职位表

        private readonly ISysEmpPosService _sysEmpPosService;
        private readonly ISysEmpExtOrgPosService _sysEmpExtOrgPosService;

        public SysPositionService(SqlSugarRepository<SysPosition> sysPosRep,
                             ISysEmpPosService sysEmpPosService,
                             ISysEmpExtOrgPosService sysEmpExtOrgPosService)
        {
            _sysPosRep = sysPosRep;
            _sysEmpPosService = sysEmpPosService;
            _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddPosition(AddPositionParam param)
        {
            var isExist = await _sysPosRep.AnyAsync(u => u.Name == param.Name || u.Code == param.Code);
            if (isExist)
                throw Oops.Oh(ErrorCode.E6000);

            var pos = param.Adapt<SysPosition>();
            await _sysPosRep.InsertAsync(pos);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task DeletePosition(DeletePositionParam param)
        {
            // 该职位下是否有员工
            var hasPosEmp = await _sysEmpPosService.HasPosEmp(param.Id);
            if (hasPosEmp)
                throw Oops.Oh(ErrorCode.E6001);

            // 该附属职位下是否有员工
            var hasExtPosEmp = await _sysEmpExtOrgPosService.HasExtPosEmp(param.Id);
            if (hasExtPosEmp)
                throw Oops.Oh(ErrorCode.E6001);

            await _sysPosRep.DeleteAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysPosition> GetPosition([FromQuery] QueryPositionParam param)
        {
            return await _sysPosRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> GetPositionList([FromQuery] PositionParam param)
        {
            return await _sysPosRep.AsQueryable()
                                    .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                                    .Where(u => u.Status != ValidityStatus.DELETED)
                                    .OrderBy(u => u.Sort).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryPositionPageList([FromQuery] PositionParam param)
        {
            var pos = await _sysPosRep.AsQueryable()
                                    .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                                    .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                                    .OrderBy(u => u.Sort)
                                    .ToPagedListAsync(param.PageNo, param.PageSize);
            return pos.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdatePosition(UpdatePositionParam param)
        {
            var isExist = await _sysPosRep.AnyAsync(u => (u.Name == param.Name || u.Code == param.Code) && u.Id != param.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.E6000);
            var pos = await _sysPosRep.FirstOrDefaultAsync(m=>m.Id == param.Id);
            pos = param.Adapt<SysPosition>();
            await _sysPosRep.AsUpdateable(pos).IgnoreColumns(ignoreAllNullColumns: true).CallEntityMethod(m => m.Modify()).ExecuteCommandAsync();
        }
    }
}
