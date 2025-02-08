using UnityEngine;
using UnityEngine.Pool;

namespace com.dhcc.pool
{
    /// <summary>
    /// A pool class designed specifically for standard C# classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class ObjectPool<T> : BasePool<T> where T : class, IPoolObject, new()
    {
        protected override T OnCreateObject()
        {
            var obj = new T();
            obj.PoolCreate();
            obj.OnReleaseToPool += Release;
            return obj;
        }
    }
}