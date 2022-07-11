using QingTian.Core;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq;
using System.Threading.Tasks;
using QingTian.Core.Entity;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysLogVisService"/>
    [Route("SysLogVis"), ApiDescriptionSettings(Name = "SysLogVis", Order = 1)]
    public class SysLogVisService : ISysLogVisService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysLogVis> _sysLogVisRep;
        public SysLogVisService(SqlSugarRepository<SysLogVis> sysLogVisRep
        )
        {
            _sysLogVisRep = sysLogVisRep;
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> Page([FromQuery] SysLogVisParam param)
        {
            var entities = await _sysLogVisRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Message), u => u.Message.Contains(param.Message.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Ip), u => u.Ip.Contains(param.Ip.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Location), u => u.Location.Contains(param.Location.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Browser), u => u.Browser.Contains(param.Browser.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Os), u => u.Os.Contains(param.Os.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Account), u => u.Account.Contains(param.Account.Trim()))
            .OrderByDescending(m => m.VisTime)
            .Select(m => new { m.Id, m.Name, m.VisType, m.Success, m.Message, m.Ip, m.Location, m.Browser, m.Os, m.VisTime })
            .ToPagedListAsync(param.Page, param.PageSize);
            return entities.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task Add(AddSysLogVisParam param)
        {
            var entity = param.Adapt<SysLogVis>();
            await _sysLogVisRep.InsertAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task Delete(DeleteSysLogVisParam param)
        {
            var entity = await _sysLogVisRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            await _sysLogVisRep.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("clearLog")]
        public async Task ClearLog()
        {
            await _sysLogVisRep.DeleteAsync(m => true);
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task Update(UpdateSysLogVisParam param)
        {
            var entity = param.Adapt<SysLogVis>();
            await _sysLogVisRep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysLogVis> Get([FromQuery] QueryeSysLogVisParam param)
        {
            return await _sysLogVisRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> List([FromQuery] SysLogVisParam param)
        {
            return await _sysLogVisRep.ToListAsync();
        }
    }
}
