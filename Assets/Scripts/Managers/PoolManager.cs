using com.dhcc.framework;
using com.dhcc.spaceshooter;
using NUnit.Framework;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;
        public static PoolManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        //[Header("References")]
        //[SerializeField] private List<>

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }
    }
}