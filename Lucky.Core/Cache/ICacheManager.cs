using System;

namespace Lucky.Core.Cache {
    public interface ICacheManager {
        TResult Get<TKey, TResult>(TKey key, Func<AcquireContext<TKey>, TResult> acquire);
        ICache<TKey, TResult> GetCache<TKey, TResult>();
    }
}
