using UnityEngine;

namespace com.dhcc.pool
{
    public abstract class SingletonPool<T> : MonoBehaviour where T : Component, IPoolObject
    {
        private static SingletonPool<T> instance;
        public static SingletonPool<T> Instance => SingletonEnforcer.OnDemand(instance);
        //{
        //    get
        //    {
        //        if(instance != null) return instance;
        //        instance = SingletonEnforcer.OnDemand<SingletonPool<T>>(Instance);
        //    }

        //}

        [Header("Settings")]
        [SerializeField] private ComponentPool<T> pool;
        [SerializeField] private bool autoCreateContainer = true;

        private void Awake()
        {
            if (SingletonEnforcer.Enforce(this, instance, out instance)) return;

            //if (Instance != null)
            //{
            //    Destroy(gameObject);
            //    return;
            //}

            //Instance = this;

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