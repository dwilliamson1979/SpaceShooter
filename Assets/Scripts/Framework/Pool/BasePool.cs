using UnityEngine;
using UnityEngine.Pool;

namespace com.dhcc.framework
{
    [System.Serializable]
    public abstract class BasePool<T> where T : class
    {
        [Header("Settings")]
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int DefaultCapacity = 10;
        [SerializeField] private int maxCapacity = 1000;
        [SerializeField] private EPoolType poolType;

        private IObjectPool<T> pool;
        public IObjectPool<T> Pool
        {
            get
            {
                if (pool == null)
                {
                    if (poolType == EPoolType.Stack)
                        pool = new UnityEngine.Pool.ObjectPool<T>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionChecks, DefaultCapacity, maxCapacity);
                    else
                        pool = new UnityEngine.Pool.LinkedPool<T>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionChecks, maxCapacity);
                }

                return pool;
            }
        }

        public void Populate(int quantity)
        {
            quantity = Mathf.Clamp(quantity, 0, maxCapacity);

            T[] tempList = new T[quantity];
            for (int i = 0; i < quantity; i++)
                tempList[i] = Pool.Get();

            for (int i = 0; i < tempList.Length; i++)
                Pool.Release(tempList[i]);

            tempList = null;
        }

        protected abstract T OnCreateObject();
        protected abstract void OnGetObject(T obj);
        protected abstract void OnReleaseObject(T obj);
        protected abstract void OnDestroyObject(T obj);
        public T Get() => Pool.Get();
    }
}