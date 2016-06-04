using System;

namespace Lucky.Core.Cache {
    public interface ICacheHolder {
        ICache<TKey, TResult> GetCache<TKey, TResult>(Type component);
    }
}
