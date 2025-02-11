using UnityEngine;

namespace com.dhcc.pool
{
    /// <summary>
    /// A class designed specifically for Unity Components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ComponentPool<T> : BasePool<T> where T : Component, IPoolObject
    {
        [Header("References")]
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;

        protected override T OnCreateObject()
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.PoolCreate();
            obj.OnReleaseToPool += Release;
            return obj;
        }

        protected override void OnDestroyObject(T obj)
        {
            obj.PoolDestroy();
            GameObject.Destroy(obj);
        }
    }
}