using com.dhcc.spaceshooter;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.framework
{
    public class PoolManager<T> : MonoBehaviour where T : System.Enum
    {
        [System.Serializable]
        public struct PoolData
        {
            public T poolIdentifier;
            public bool collectionChecks;
            public int defaultCapacity;
            public int maxCapacity;
            public EPoolType poolType;
            public bool defaultActiveStatus;
            public bool usePoolObjectIfAvailable;
            public GameObject prefab;
            public int prefillAmount;
            public bool autoCreateContainer;
        }

        private static PoolManager<T> instance;
        public static PoolManager<T> Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [SerializeField] private List<PoolData> poolList;
        private Dictionary<T, GameObjectPool> pools = new();

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            foreach (var poolEntry in poolList)
            {
                if (pools.ContainsKey(poolEntry.poolIdentifier))
                {
                    Debug.LogWarning($"PoolManager: an attempt to add a duplicate pool ({poolEntry.poolIdentifier}) has been blocked.");
                    continue;
                }

                Transform container = null;
                if (poolEntry.autoCreateContainer)
                {
                    GameObject go = new GameObject($"{poolEntry.poolIdentifier} (Pool)");
                    go.transform.parent = transform;
                    container = go.transform;
                }
                else
                    container = transform;

                pools[poolEntry.poolIdentifier] = new GameObjectPool(poolEntry.prefab, container, poolEntry.collectionChecks, poolEntry.defaultCapacity, poolEntry.maxCapacity, poolEntry.poolType);
                pools[poolEntry.poolIdentifier].Populate(poolEntry.prefillAmount);
            }
        }

        public void AddPool(T poolIdentifier, GameObjectPool pool)
        {
            if (pools.ContainsKey(poolIdentifier))
            {
                Debug.LogWarning($"PoolManager: an attempt to add a duplicate pool ({poolIdentifier}) has been blocked.");
                return;
            }

            pools[poolIdentifier] = pool;
        }

        public void RemovePool(T poolIdentifier)
        {
            if (!pools.ContainsKey(poolIdentifier)) return;

            pools.Remove(poolIdentifier);
        }

        public static GameObject Get(T poolIdentifier)
        {
            if (Instance.pools.TryGetValue(poolIdentifier, out GameObjectPool pool))
                return pool.GetGameObject();

            return null;
        }

        public static C Get<C>(T poolIdentifier) where C : Component
        {
            if (Instance.pools.TryGetValue(poolIdentifier, out GameObjectPool pool))
                return pool.GetComponent<C>();

            return null;
        }
    }
}