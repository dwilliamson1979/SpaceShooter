using UnityEngine;

namespace com.dhcc.framework
{
    /// <summary>
    /// Provides an easy way to add and track objects which have been set to DontDestroyOnLoad().*/
    /// </summary>
    public class DontDestroy : MonoBehaviour
    {
        public static void Add(GameObject go)
        {
            if (go != null && go.GetComponent<DontDestroy>() == null)
                go.AddComponent<DontDestroy>();
        }

        public static bool IsDontDestroy(GameObject go)
        {
            return (go != null) ? (go.GetComponent<DontDestroy>() != null) : false;
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
