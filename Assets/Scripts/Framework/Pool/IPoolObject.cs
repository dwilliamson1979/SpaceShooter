using System;

namespace com.dhcc.framework
{
    public interface IPoolObject
    {
        event Action PoolRelease;

        void PoolOnCreate();
        void PoolOnGet();
        void PoolOnRelease();
        void PoolOnDestroy();
    }
}