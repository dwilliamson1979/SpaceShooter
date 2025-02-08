using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace com.dhcc.pool
{
    public interface IPoolObject
    {
        event Action<IPoolObject> OnReleaseToPool;

        void PoolCreate();
        void PoolGet();
        void PoolRelease();
        void PoolDestroy();
    }
}