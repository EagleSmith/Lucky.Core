using System;

namespace Lucky.Core.Cache {
    public interface ICache<TKey, TResult> {
        TResult Get(TKey key, Func<AcquireContext<TKey>, TResult> acquire);
    }
}
