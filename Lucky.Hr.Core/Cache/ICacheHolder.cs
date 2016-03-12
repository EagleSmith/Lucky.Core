using System;

namespace Lucky.Hr.Caching {
    public interface ICacheHolder {
        ICache<TKey, TResult> GetCache<TKey, TResult>(Type component);
    }
}
