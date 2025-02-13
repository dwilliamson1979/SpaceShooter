using UnityEngine;

namespace com.dhcc.pool
{
    public abstract class SingletonPool<T> : MonoBehaviour where T : Component, IPoolObject
    {
        public static SingletonPool<T> Instance;

        [Header("Settings")]
        [SerializeField] private ComponentPool<T> pool;
        [SerializeField] private bool autoCreateContainer = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

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