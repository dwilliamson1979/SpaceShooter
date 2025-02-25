using UnityEngine;

namespace com.dhcc.eventsystem
{
    public class LocalEventBus : MonoBehaviour
    {
        public EventSystem EventSystem { get; } = new();

        public static LocalEventBus Get(GameObject go)
        {
            return go.GetOrAdd<LocalEventBus>();
        }
    }
}