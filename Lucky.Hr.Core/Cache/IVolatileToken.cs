namespace Lucky.Hr.Caching {
    public interface IVolatileToken {
        bool IsCurrent { get; }
    }
}