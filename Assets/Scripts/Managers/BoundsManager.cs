using com.dhcc.framework;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class BoundsManager : MonoBehaviour
    {
        private static BoundsManager instance;
        public static BoundsManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [Header("Settings")]
        [SerializeField] private Boundary arenaBoundary;

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }

        //Wrappers to avoid copying the struct.
        public bool IsOutOfBounds(Transform transform) => arenaBoundary.IsOutOfBounds(transform);
        public bool IsOutOfLeftBounds(float val) => arenaBoundary.IsOutOfLeftBounds(val);
        public bool IsOutOfRightBounds(float val) => arenaBoundary.IsOutOfRightBounds(val);
        public bool IsOutOfBottomBounds(float val) => arenaBoundary.IsOutOfBottomBounds(val);
        public bool IsOutOfTopBounds(float val) => arenaBoundary.IsOutOfTopBounds(val);
    }
}