using UnityEngine;
using com.dhcc.utility;

namespace com.dhcc.pool
{
    public abstract class SingletonPool<T> : MonoBehaviour where T : Component, IPoolObject
    {
        private static SingletonPool<T> instance;
        public static SingletonPool<T> Instance => SingletonEmulator.Get(instance);

        [Header("Settings")]
        [SerializeField] private ComponentPool<T> pool;
        [SerializeField] private bool autoCreateContainer = true;

        private void Awake()
        {
            if (SingletonEmulator.Enforce(this, instance, out instance)) return;

            if(autoCreateContainer)
            {
                GameObject container = new GameObject("Container");
                container.transform.SetParent(transform);
                pool.SetContainer(container.transform);
            }
        }

        public static T Get() => Instance.pool.Get();
    }
}