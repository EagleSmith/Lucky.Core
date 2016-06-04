namespace Lucky.Core.Cache {
    public interface ICacheContextAccessor {
        IAcquireContext Current { get; set; }
    }
}