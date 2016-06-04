using System;
using System.Collections.Generic;
using System.Linq;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Lucky.Core.Cache.Memcached
{
    public class MemCache<TKey, TResult> : ICache<TKey, TResult>
    {
        private readonly ICacheContextAccessor _cacheContextAccessor;
        private static MemcachedClient _client;

        public MemCache(ICacheContextAccessor cacheContextAccessor)
        {
            _cacheContextAccessor = cacheContextAccessor;
            _client = MemcachedHelper.GetInstance();
        }

        public TResult Get(TKey key, Func<AcquireContext<TKey>, TResult> acquire)
        {
           
                CacheEntry entity = null;
                TResult t ;
                var r = _client.ExecuteGet<CacheEntry>(key.ToString());
                if (r.Value == null)
                {
                    entity = AddEntry(key, acquire);
                    t = entity.Result;
                }
                else
                {
                    entity=UpdateEntry(r.Value, key, acquire);
                    t = entity.Result;
                }
                return t;
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
                //创建缓存
                var flag = _client.ExecuteStore(StoreMode.Set, k.ToString(), entry);
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
        protected class CacheEntry
        {
            private IList<IVolatileToken> _tokens;
            public TResult Result { get; set; }

            public IEnumerable<IVolatileToken> Tokens
            {
                get { return _tokens ?? Enumerable.Empty<IVolatileToken>(); }
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
