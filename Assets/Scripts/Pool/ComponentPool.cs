using UnityEngine;
using UnityEngine.Pool;

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
        [SerializeField] private Transform container;

        protected override T OnCreateObject()
        {
            T obj = GameObject.Instantiate(prefab, container);
            obj.PoolCreate();
            obj.OnReleaseToPool += () => Pool.Release(obj);

            return obj;
        }

        protected override void OnDestroyObject(T obj)
        {
            obj.PoolDestroy();
        }

        public void SetContainer(Transform container) => this.container = container;
    }
}