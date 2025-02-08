
namespace com.dhcc.pool
{
    /// <summary>
    /// A relic. This may go away. The idea was to have a generic interface for Pools...but I haven't found a reason for a pooled object to have knowledge of its pool...
    /// </summary>
    public interface IPool
    {
        void Release(IPoolObject obj);
    }
}