using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <inheritdoc cref="ISysCacheService"/>
    [Route("Cache"), ApiDescriptionSettings(Name = "Cache", Order = 0)]
    public class SysCacheService : ISysCacheService, IDynamicApiController, ISingleton
    {
        private readonly ICache _cache;
        private readonly CacheOptions _cacheOptions;

        public SysCacheService(IOptions<CacheOptions> cacheOptions, Func<string, ISingleton, object> resolveNamed)
        {
            _cacheOptions = cacheOptions.Value;
            _cache = resolveNamed(_cacheOptions.CacheType.ToString(), default) as ICache;
        }

        /// <inheritdoc/>
        public async Task<bool> SetAsync(string key, object value)
        {
            return await _cache.SetAsync(key, value);
        }

        /// <inheritdoc/>
        [HttpGet("getDataScope")]
        public async Task<List<long>> GetDataScope(long userId)
        {
            var cacheKey = ConstCache.CACHE_KEY_DATASCOPE + $"{userId}";
            return await _cache.GetAsync<List<long>>(cacheKey);
        }

        /// <inheritdoc/>
        public async Task<List<long>> GetUsersDataScope(long userId)
        {
            var cacheKey = ConstCache.CACHE_KEY_USERSDATASCOPE + $"{userId}";
            return await _cache.GetAsync<List<long>>(cacheKey);
        }

        /// <inheritdoc/>
        public async Task<List<MenuTreeNode>> GetMenuAsync(long userId)
        {
            var cacheKey = ConstCache.CACHE_KEY_MENU + $"{userId}";
            return await _cache.GetAsync<List<MenuTreeNode>>(cacheKey);
        }

        /// <inheritdoc/>
        public async Task<List<string>> GetPermissionAsync(long userId)
        {
            var cacheKey = ConstCache.CACHE_KEY_PERMISSION + $"{userId}";
            return await _cache.GetAsync<List<string>>(cacheKey);
        }

        /// <inheritdoc/>
        public List<string> GetAllCacheKeys()
        {
            var cacheItems = _cache.GetAllKeys();
            if (cacheItems == null) return new List<string>();
            return cacheItems.Where(u => !u.ToString().StartsWith("mini-profiler")).Select(u => u).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<string>> GetAllPermissionAsync()
        {
            var cacheKey = ConstCache.CACHE_KEY_ALLPERMISSION;
            return await _cache.GetAsync<List<string>>(cacheKey);
        }

        /// <inheritdoc/>
        public async Task<bool> DelAsync(string key)
        {
            await _cache.DelAsync(key);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DelByStartsWithAsync(string key)
        {
            await _cache.DelByStartsWithAsync(key);
            return true;
        }

        /// <inheritdoc/>
        public async Task SetAllPermissionAsync(List<string> permissions)
        {
            var cacheKey = ConstCache.CACHE_KEY_ALLPERMISSION;
            await _cache.SetAsync(cacheKey, permissions);
        }

        /// <inheritdoc/>
        public async Task<string> GetAsync(string key)
        {
            return await _cache.GetAsync(key);
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            return await _cache.GetAsync<T>(key);
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string key)
        {
            return _cache.ExistsAsync(key);
        }

        /// <inheritdoc/>
        [NonAction]
        public bool Del(string key)
        {
            _cache.Del(key);
            return true;
        }

        /// <inheritdoc/>
        [NonAction]
        public bool Exists(string key)
        {
            return _cache.Exists(key);
        }

        /// <inheritdoc/>
        [NonAction]
        public string Get(string key)
        {
            return _cache.Get(key);
        }

        /// <inheritdoc/>
        [NonAction]
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task SetMenuAsync(long userId, List<MenuTreeNode> menus)
        {
            var cacheKey = ConstCache.CACHE_KEY_MENU + $"{userId}";
            await _cache.SetAsync(cacheKey, menus);
        }

        /// <inheritdoc/>
        [NonAction]
        public bool Set(string key, object value)
        {
            return _cache.Set(key, value);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task SetPermissionAsync(long userId, List<string> permissions)
        {
            var cacheKey = ConstCache.CACHE_KEY_PERMISSION + $"{userId}";
            await _cache.SetAsync(cacheKey, permissions);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task SetUsersDataScope(long userId, List<long> dataScopes)
        {
            var cacheKey = ConstCache.CACHE_KEY_USERSDATASCOPE + $"{userId}";
            await _cache.SetAsync(cacheKey, dataScopes);
        }

        /// <inheritdoc/>
        [NonAction]
        public async Task SetDataScope(long userId, List<long> dataScopes)
        {
            var cacheKey = ConstCache.CACHE_KEY_DATASCOPE + $"{userId}";
            await _cache.SetAsync(cacheKey, dataScopes);
        }
    }
}
