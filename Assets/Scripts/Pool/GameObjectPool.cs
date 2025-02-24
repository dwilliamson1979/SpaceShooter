using UnityEngine;
using UnityEngine.Pool;

namespace com.dhcc.pool
{

    [System.Serializable]
    public class GameObjectPool
    {
        [Header("References")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform container;

        [Header("Settings")]
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int DefaultCapacity = 10;
        [SerializeField] private int maxCapacity = 1000;
        [SerializeField] private EPoolType poolType;
        [SerializeField] private bool defaultActiveStatus = false;

        private IObjectPool<GameObject> pool;
        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (pool == null)
                {
                    if (poolType == EPoolType.Stack)
                        pool = new UnityEngine.Pool.ObjectPool<GameObject>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionChecks, DefaultCapacity, maxCapacity);
                    else
                        pool = new UnityEngine.Pool.LinkedPool<GameObject>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionChecks, maxCapacity);
                }

                return pool;
            }
        }

        public void Populate(int quantity)
        {
            quantity = Mathf.Clamp(quantity, 0, maxCapacity);

            GameObject[] tempList = new GameObject[quantity];
            for (int i = 0; i < quantity; i++)
                tempList[i] = Pool.Get();

            for (int i = 0; i < tempList.Length; i++)
                Pool.Release(tempList[i]);

            tempList = null;
        }

        private GameObject OnCreateObject()
        {
            GameObject go = GameObject.Instantiate(prefab, container);

            IPoolObject po = go.GetComponent<IPoolObject>();
            if (po != null)
            {
                po.PoolOnCreate();
                po.ReleaseToPool += () => Pool.Release(go);
            }

            go.SetActive(defaultActiveStatus);

            return go;
        }

        private void OnGetObject(GameObject go)
        {
            IPoolObject po = go.GetComponent<IPoolObject>();
            if (po != null)
                po.PoolOnGet();
        }

        private void OnReleaseObject(GameObject go)
        {
            IPoolObject po = go.GetComponent<IPoolObject>();
            if (po != null)
                po.PoolOnRelease();
        }

        private void OnDestroyObject(GameObject go)
        {
            IPoolObject po = go.GetComponent<IPoolObject>();
            if (po != null)
                po.PoolOnDestroy();
        }

        public GameObject Get() => Pool.Get();
    }
}