using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace com.dhcc.pool
{
    public interface IPoolObject
    {
        void PoolCreate(IPool pool);
        void CheckOut();
        void CheckIn();
        void PoolDestroy();
    }
}