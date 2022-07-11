using Furion;
using Furion.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace QingTian.Core.Cache
{
    public class SqlSugarCache : ICacheService
    {
        private static readonly ICache _cache = App.GetOptions<CacheOptions>().CacheType == CacheType.MemoryCache ? App.RootServices.GetService(typeof(MemoryCache)) as ICache : App.RootServices.GetService(typeof(RedisCache)) as ICache;

        public void Add<V>(string key, V value)
        {
            _cache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
        }

        public bool ContainsKey<V>(string key)
        {
            return _cache.Exists(key);
        }

        public V Get<V>(string key)
        {
            return _cache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return _cache.GetAllKeys();
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<V>(cacheKey))
            {
                return this.Get<V>(cacheKey);
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            _cache.Del(key);
        }
    }
}
