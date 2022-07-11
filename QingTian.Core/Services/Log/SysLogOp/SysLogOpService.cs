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
    /// <inheritdoc cref="ISysLogOpService"/>
    [Route("SysLogOp"), ApiDescriptionSettings(Name = "SysLogOp", Order = 1)]
    public class SysLogOpService : ISysLogOpService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysLogOp> _sysLogOpRep;
        public SysLogOpService(SqlSugarRepository<SysLogOp> sysLogOpRep
        )
        {
            _sysLogOpRep = sysLogOpRep;
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> Page([FromQuery] SysLogOpParam param)
        {
            var entities = await _sysLogOpRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Message), u => u.Message.Contains(param.Message.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Ip), u => u.Ip.Contains(param.Ip.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Location), u => u.Location.Contains(param.Location.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Browser), u => u.Browser.Contains(param.Browser.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Os), u => u.Os.Contains(param.Os.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Url), u => u.Url.Contains(param.Url.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ClassName), u => u.ClassName.Contains(param.ClassName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.MethodName), u => u.MethodName.Contains(param.MethodName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.ReqMethod), u => u.ReqMethod.Contains(param.ReqMethod.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Param), u => u.Param.Contains(param.Param.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Result), u => u.Result.Contains(param.Result.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(param.Account), u => u.Account.ToLower() == param.Account.Trim().ToLower())
            .OrderByDescending(m => m.OpTime)
            .Select(m => new { m.Id, m.ReqMethod, m.Success, m.Ip, m.Browser, m.Os, m.Url, m.Name, m.ElapsedTime, m.OpTime })
            .ToPagedListAsync(param.Page, param.PageSize);
            return entities.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task Add(AddSysLogOpParam param)
        {
            var entity = param.Adapt<SysLogOp>();
            await _sysLogOpRep.InsertAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("delete")]
        public async Task Delete(DeleteSysLogOpParam param)
        {
            var entity = await _sysLogOpRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            await _sysLogOpRep.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        [HttpPost("clearLog")]
        public async Task ClearLog()
        {
            await _sysLogOpRep.DeleteAsync(m => true);
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task Update(UpdateSysLogOpParam param)
        {
            var entity = param.Adapt<SysLogOp>();
            await _sysLogOpRep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysLogOp> Get([FromQuery] QueryeSysLogOpParam param)
        {
            return await _sysLogOpRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> List([FromQuery] SysLogOpParam param)
        {
            return await _sysLogOpRep.ToListAsync();
        }
    }
}
