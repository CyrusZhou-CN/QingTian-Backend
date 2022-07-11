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
    /// <inheritdoc cref="ISysLogExService"/>
    [Route("SysLogEx"), ApiDescriptionSettings(Name = "SysLogEx", Order = 1)]
    public class SysLogExService : ISysLogExService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysLogEx> _sysLogExRep;
        public SysLogExService(SqlSugarRepository<SysLogEx> sysLogExRep
        )
        {
            _sysLogExRep = sysLogExRep;
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> Page([FromQuery] SysLogExParam param)
        {
            var entities = await _sysLogExRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(param.Account), u => u.Account.Contains(param.Account.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ClassName), u => u.ClassName.Contains(param.ClassName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.MethodName), u => u.MethodName.Contains(param.MethodName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ExceptionName), u => u.ExceptionName.Contains(param.ExceptionName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ExceptionMsg), u => u.ExceptionMsg.Contains(param.ExceptionMsg.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ExceptionSource), u => u.ExceptionSource.Contains(param.ExceptionSource.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.StackTrace), u => u.StackTrace.Contains(param.StackTrace.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ParamsObj), u => u.ParamsObj.Contains(param.ParamsObj.Trim()))
            .OrderByDescending(m => m.ExceptionTime)
            .Select(m => new { m.Id, m.Name, m.ClassName, m.MethodName, m.ExceptionName, m.ExceptionMsg, m.ParamsObj, m.ExceptionTime })
            .ToPagedListAsync(param.Page, param.PageSize);
            return entities.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task Add(AddSysLogExParam param)
        {
            var entity = param.Adapt<SysLogEx>();
            await _sysLogExRep.InsertAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task Delete(DeleteSysLogExParam param)
        {
            var entity = await _sysLogExRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            await _sysLogExRep.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("clearLog")]
        public async Task ClearLog()
        {
            await _sysLogExRep.DeleteAsync(m => true);
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task Update(UpdateSysLogExParam param)
        {
            var entity = param.Adapt<SysLogEx>();
            await _sysLogExRep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysLogEx> Get([FromQuery] QueryeSysLogExParam param)
        {
            return await _sysLogExRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> List([FromQuery] SysLogExParam param)
        {
            return await _sysLogExRep.ToListAsync();
        }
    }
}
