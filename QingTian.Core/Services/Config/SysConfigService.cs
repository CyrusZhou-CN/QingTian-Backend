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

namespace QingTian.Core.Services.Config
{

    /// <inheritdoc cref="ISysConfigService"/>
    [Route("sysConfig"), ApiDescriptionSettings(Name = "Config", Order = 0)]
    public class SysConfigService : ISysConfigService, IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysConfig> _sysConfigRep;    // 参数配置表
        private readonly ISysCacheService _sysCacheService;

        public SysConfigService(SqlSugarRepository<SysConfig> sysConfigRep, ISysCacheService sysCacheService)
        {
            _sysConfigRep = sysConfigRep;
            _sysCacheService = sysCacheService;
        }

        /// <inheritdoc/>
        [HttpGet("detail")]
        public async Task<SysConfig> GetConfigAsync([FromQuery] QueryConfigParam param)
        {
            return await _sysConfigRep.FirstOrDefaultAsync(u => u.Id == param.Id);
        }

        /// <inheritdoc/>
        [HttpGet("list")]
        public async Task<dynamic> GetConfigListAsync()
        {
            return await _sysConfigRep.Where(u => u.Status != ValidityStatus.DELETED).ToListAsync();
        }

        /// <inheritdoc/>
        [HttpGet("page")]
        public async Task<dynamic> QueryConfigPageListAsync([FromQuery] ConfigParam param)
        {
            var configs = await _sysConfigRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(param.Name), u => u.Name.Contains(param.Name.Trim()))
                .WhereIF(!string.IsNullOrWhiteSpace(param.Code), u => u.Code.Contains(param.Code.Trim()))
                .WhereIF(!string.IsNullOrWhiteSpace(param.GroupCode), u => u.GroupCode == param.GroupCode.Trim())
                 .Where(u => u.Status != ValidityStatus.DELETED).OrderBy(u => u.GroupCode)
                 .ToPagedListAsync(param.Page, param.PageSize);
            return configs.QtPagedResult();
        }

        /// <inheritdoc/>
        [HttpPost("add")]
        public async Task AddConfig(AddConfigParam param)
        {
            var isExist = await _sysConfigRep.AnyAsync(u => u.Name == param.Name || u.Code == param.Code);
            if (isExist)
                throw Oops.Oh(ErrorCode.E0005);

            var config = param.Adapt<SysConfig>();
            await _sysConfigRep.InsertAsync(config);
        }

        /// <inheritdoc/>
        [HttpPost("del")]
        public async Task DeleteConfigAsync(DeleteConfigParam param)
        {
            var config = await _sysConfigRep.FirstOrDefaultAsync(u => u.Id == param.Id);
            // 禁止删除系统参数
            if (config.SysFlag == YesOrNo.Yes)
                throw Oops.Oh(ErrorCode.E0006);

            await _sysConfigRep.DeleteAsync(config);
            //刷新缓存
            await _sysCacheService.DelAsync(config.Code);
        }

        /// <inheritdoc/>
        [HttpPost("edit")]
        public async Task UpdateConfigAsync(UpdateConfigParam param)
        {
            var isExist = await _sysConfigRep.AnyAsync(u => (u.Name == param.Name || u.Code == param.Code) && u.Id != param.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.E0005);

            var config = param.Adapt<SysConfig>();
            await _sysConfigRep.AsUpdateable(config).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //刷新缓存
            await _sysCacheService.SetAsync(param.Code, param.Value);
        }

        /// <inheritdoc/>
        public async Task UpdateConfigCacheAsync(string code, object value)
        {
            await _sysCacheService.SetAsync(code, value);
        }

        /// <inheritdoc/>
        public async Task<string> GetDefaultPasswordAsync()
        {
            var value = await GetConfigCacheAsync("DEFAULT_PASSWORD");
            return value;
        }

        /// <inheritdoc/>
        public async Task<bool> GetCaptchaOpenFlagAsync()
        {
            var value = await GetConfigCacheAsync("CAPTCHA_OPEN");
            if (string.IsNullOrWhiteSpace(value)) return false;
            return bool.Parse(value);
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<dynamic> GetConfigCacheAsync(string code)
        {
            var value = await _sysCacheService.GetAsync(code);
            if (string.IsNullOrEmpty(value))
            {
                var config = await _sysConfigRep.FirstOrDefaultAsync(u => u.Code == code);
                value = config != null ? config.Value : "";
                await _sysCacheService.SetAsync(code, value);
            }
            return value;
        }
    }
}
