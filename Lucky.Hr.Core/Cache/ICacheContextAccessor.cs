namespace Lucky.Hr.Caching {
    public interface ICacheContextAccessor {
        IAcquireContext Current { get; set; }
    }
}