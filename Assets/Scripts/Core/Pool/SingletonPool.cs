using UnityEngine;

namespace com.dhcc.pool
{
    public abstract class SingletonPool<T> : MonoBehaviour where T : Component, IPoolObject
    {
        public static SingletonPool<T> Instance;

        [Header("Settings")]
        [SerializeField] private ComponentPool<T> pool;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public static T Get() => Instance.pool.Get();
    }
}