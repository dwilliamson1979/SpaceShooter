using NUnit.Framework;
using UnityEngine;

namespace com.dhcc.framework
{
    [CreateAssetMenu(fileName = "PoolListSO", menuName = "Pools/PoolListSO")]
    public class PoolListSO : ScriptableObject
    {
        [System.Serializable]
        private struct PoolListEntry<T> where T : System.Enum
        {
            public T name;
            public GameObject poolPrefab;
        }


        //public List<>
    }
}