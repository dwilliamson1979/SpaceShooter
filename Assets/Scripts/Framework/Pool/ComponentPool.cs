using UnityEngine;

namespace com.dhcc.framework
{
    /// <summary>
    /// A class designed specifically for Unity Components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ComponentPool<T> : BasePool<T> where T : Component, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private bool defaultActiveStatus = false;

        [Header("References")]
        [SerializeField] private T prefab;
        [SerializeField] private Transform container;

        protected override T OnCreateObject()
        {
            T obj = GameObject.Instantiate(prefab, container);
            obj.ReleaseToPool += () => Pool.Release(obj);
            obj.PoolOnCreate();            
            obj.gameObject.SetActive(defaultActiveStatus);
            return obj;
        }

        protected override void OnGetObject(T obj) => obj.PoolOnGet();
        protected override void OnReleaseObject(T obj) => obj.PoolOnRelease();
        protected override void OnDestroyObject(T obj) => obj.PoolOnDestroy();
    }
}