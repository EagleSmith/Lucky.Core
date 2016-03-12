using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Caching;
using Lucky.Hr.Core.Infrastructure;
using Lucky.Hr.Core.Utility;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace Lucky.Hr.Core.Cache.RedisCache
{
    public class RedisCache<TKey, TResult> : ICache<TKey, TResult>
    {
        private readonly ICacheContextAccessor _cacheContextAccessor;
        private static ICacheClient _client;
        public RedisCache(ICacheContextAccessor cacheContextAccessor)
        {
            _cacheContextAccessor = cacheContextAccessor;
            _client = EngineContext.Current.Resolve<ICacheClient>();
        }

        public TResult Get(TKey key, Func<AcquireContext<TKey>, TResult> acquire)
        {
            CacheEntry entity = null;
            entity = _client.Get<CacheEntry>(key.ToString());
            if (!_client.Exists(key.ToString()))
            {
                entity = AddEntry(key, acquire);
            }
            else
            {
                entity = UpdateEntry(entity, key, acquire);
            }
            return entity.Result;
        }

        private CacheEntry AddEntry(TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = CreateEntry(k, acquire);
            PropagateTokens(entry);
            return entry;
        }
        private CacheEntry UpdateEntry(CacheEntry currentEntry, TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = (currentEntry.Tokens.Any(t => t != null && !t.IsCurrent)) ? CreateEntry(k, acquire) : currentEntry;
            PropagateTokens(entry);
            return entry;
        }
        private void PropagateTokens(CacheEntry entry)
        {
            // Bubble up volatile tokens to parent context
            if (_cacheContextAccessor.Current != null)
            {
                foreach (var token in entry.Tokens)
                    _cacheContextAccessor.Current.Monitor(token);
            }
        }


        private CacheEntry CreateEntry(TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = new CacheEntry();
            var context = new AcquireContext<TKey>(k, entry.AddToken);

            IAcquireContext parentContext = null;
            try
            {
                // Push context
                parentContext = _cacheContextAccessor.Current;
                _cacheContextAccessor.Current = context;

                entry.Result = acquire(context);
                _client.Add(k.ToString(), entry);
            }
            finally
            {
                // Pop context
                _cacheContextAccessor.Current = parentContext;
            }
            entry.CompactTokens();
            return entry;
        }
        [Serializable]
        private class CacheEntry
        {
            private IList<IVolatileToken> _tokens;
            public TResult Result { get; set; }

            public IEnumerable<IVolatileToken> Tokens
            {
                get
                {
                    return _tokens ?? Enumerable.Empty<IVolatileToken>();
                }
            }

            public void AddToken(IVolatileToken volatileToken)
            {
                if (_tokens == null)
                {
                    _tokens = new List<IVolatileToken>();
                }

                _tokens.Add(volatileToken);
            }

            public void CompactTokens()
            {
                if (_tokens != null)
                    _tokens = _tokens.Distinct().ToArray();
            }
        }
        
    }
}
