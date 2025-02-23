using System;

namespace com.dhcc.pool
{
    public interface IPoolObject
    {
        event Action OnReleaseToPool;

        void PoolCreate();
        void PoolGet();
        void PoolRelease();
        void PoolDestroy();
    }
}