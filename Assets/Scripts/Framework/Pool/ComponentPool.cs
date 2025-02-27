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
            obj.PoolOnCreate();
            obj.ReleaseToPool += () => Pool.Release(obj);
            obj.gameObject.SetActive(defaultActiveStatus);

            return obj;
        }

        //public void SetContainer(Transform container) => this.container = container;
    }
}