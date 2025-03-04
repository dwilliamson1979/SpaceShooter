using UnityEngine;

namespace com.dhcc.framework
{

    [System.Serializable]
    public class GameObjectPool : BasePool<GameObject>
    {
        [Header("Settings")]
        [SerializeField] private bool defaultActiveStatus = false;
        [SerializeField] private bool usePoolObjectIfAvailable = true;

        [Header("References")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform container;

        protected override GameObject OnCreateObject()
        {
            GameObject go = GameObject.Instantiate(prefab, container);

            if (usePoolObjectIfAvailable)
            {
                IPoolObject po = go.GetComponent<IPoolObject>();
                if (po != null)
                {
                    po.PoolOnCreate();
                    po.ReleaseToPool += () => Pool.Release(go);
                }
            }

            go.SetActive(defaultActiveStatus);
            return go;
        }

        protected override void OnGetObject(GameObject obj)
        {
            if (usePoolObjectIfAvailable)
            {
                IPoolObject po = obj.GetComponent<IPoolObject>();
                if (po != null)
                {
                    po.PoolOnGet();
                    return;
                }
            }

            obj.SetActive(true);
        }

        protected override void OnReleaseObject(GameObject obj)
        {
            if (usePoolObjectIfAvailable)
            {
                IPoolObject po = obj.GetComponent<IPoolObject>();
                if (po != null)
                {
                    po.PoolOnRelease();
                    return;
                }
            }

            obj.SetActive(false);
        }

        protected override void OnDestroyObject(GameObject obj)
        {
            if (usePoolObjectIfAvailable)
            {
                IPoolObject po = obj.GetComponent<IPoolObject>();
                if (po != null)
                    po.PoolOnDestroy();
            }
        }

        public void Release(GameObject obj) => Pool.Release(obj);
    }
}