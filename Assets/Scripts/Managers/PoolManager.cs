using com.dhcc.framework;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    [System.Serializable]
    public enum EPoolIdentifier
    {
        Laser,
        Enemy
    }

    [System.Serializable]
    public struct PoolEntry
    {
        public EPoolIdentifier poolIdentifier;
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

    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;
        public static PoolManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [SerializeField] private List<PoolEntry> poolList;
        private Dictionary<EPoolIdentifier, GameObjectPool> pools = new();

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

        public void AddPool(EPoolIdentifier poolIdentifier, GameObjectPool pool)
        {
            if (pools.ContainsKey(poolIdentifier))
            {
                Debug.LogWarning($"PoolManager: an attempt to add a duplicate pool ({poolIdentifier}) has been blocked.");
                return;
            }

            pools[poolIdentifier] = pool;
        }

        public void RemovePool(EPoolIdentifier poolIdentifier)
        {
            if (!pools.ContainsKey(poolIdentifier)) return;

            pools.Remove(poolIdentifier);
        }

        public static GameObject Get(EPoolIdentifier poolIdentifier)
        {
            if (Instance.pools.TryGetValue(poolIdentifier, out GameObjectPool pool))
                return pool.GetGameObject();

            return null;
        }

        public static T Get<T>(EPoolIdentifier poolIdentifier) where T : Component
        {
            if (Instance.pools.TryGetValue(poolIdentifier, out GameObjectPool pool))
                return pool.GetComponent<T>();

            return null;
        }
    }
}