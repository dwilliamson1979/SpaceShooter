
namespace com.dhcc.framework
{
    /// <summary>
    /// A pool class designed specifically for standard C# classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ClassPool<T> : BasePool<T> where T : class, IPoolObject, new()
    {
        protected override T OnCreateObject()
        {
            var obj = new T();            
            obj.ReleaseToPool += () => Pool.Release(obj);
            obj.PoolOnCreate();
            return obj;
        }

        protected override void OnGetObject(T obj) => obj.PoolOnGet();
        protected override void OnReleaseObject(T obj) => obj.PoolOnRelease();
        protected override void OnDestroyObject(T obj) => obj.PoolOnDestroy();
    }
}