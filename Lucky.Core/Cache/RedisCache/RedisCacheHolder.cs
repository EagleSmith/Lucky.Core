using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core;

namespace Lucky.Core.Cache.RedisCache
{

    public interface IRedisCacheHolder : ICacheHolder
    {

    }
    public class RedisCacheHolder : IRedisCacheHolder
    {
        private readonly ICacheContextAccessor _cacheContextAccessor;
        private readonly ConcurrentDictionary<CacheKey, object> _caches = new ConcurrentDictionary<CacheKey, object>();
        private ICacheClient _client;
        public RedisCacheHolder(ICacheContextAccessor cacheContextAccessor, ICacheClient client)
        {
            _cacheContextAccessor = cacheContextAccessor;
            _client = client;
        }
        private class CacheKey : Tuple<Type, Type, Type>
        {
            public CacheKey(Type component, Type key, Type result)
                : base(component, key, result) { }
        }
        public ICache<TKey, TResult> GetCache<TKey, TResult>(Type component)
        {
            var cacheKey = new CacheKey(component, typeof(TKey), typeof(TResult));
            var result = _caches.GetOrAdd(cacheKey, k => new RedisCache<TKey, TResult>(_cacheContextAccessor,_client));
            return (RedisCache<TKey, TResult>)result;
        }
    }
}
