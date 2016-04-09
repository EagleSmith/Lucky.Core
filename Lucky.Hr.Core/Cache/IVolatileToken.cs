namespace Lucky.Core.Cache {
    public interface IVolatileToken {
        bool IsCurrent { get; }
    }
}