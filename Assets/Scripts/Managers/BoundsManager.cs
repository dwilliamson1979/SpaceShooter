using com.dhcc.framework;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class BoundsManager : MonoBehaviour
    {
        private static BoundsManager instance;
        public static BoundsManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [Header("Settings")]
        [field: SerializeField] public Vector2 HorizontalBoundary { get; private set; }
        [field: SerializeField] public Vector2 VerticalBoundary { get; private set; }

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }

        public bool IsOutOfBounds(Transform transform)
        {
            return transform.position.x < HorizontalBoundary.x
                || transform.position.x > HorizontalBoundary.y
                || transform.position.y < VerticalBoundary.x
                || transform.position.y > HorizontalBoundary.y;
        }
    }
}