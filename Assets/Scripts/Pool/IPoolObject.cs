using System;

namespace com.dhcc.pool
{
    public interface IPoolObject
    {
        event Action ReleaseToPool;

        void PoolOnCreate();
        void PoolOnGet();
        void PoolOnRelease();
        void PoolOnDestroy();
    }
}