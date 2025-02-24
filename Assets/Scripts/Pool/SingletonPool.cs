using UnityEngine;
using com.dhcc.utility;

namespace com.dhcc.pool
{
    public abstract class SingletonPool<T> : MonoBehaviour where T : Component, IPoolObject
    {
        private static SingletonPool<T> instance;
        public static SingletonPool<T> Instance => SingletonEmulator.Get(instance);

        [Header("Pool Settings")]
        [SerializeField] private ComponentPool<T> pool;
        [SerializeField] protected int prefill = 0;

        private void Awake()
        {
            if (SingletonEmulator.Enforce(this, instance, out instance)) return;

            //pool.SetContainer(transform);
            pool.Populate(prefill);
        }

        public static T Get() => Instance.pool.Get();
    }
}