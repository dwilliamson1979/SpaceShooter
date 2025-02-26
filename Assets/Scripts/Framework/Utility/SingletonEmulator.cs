using UnityEngine;

namespace com.dhcc.framework
{
    public class SingletonEmulator : Singleton<SingletonEmulator>
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// This method creates a new singleton on demand if the instance is null. Coupled with Enforce(), this allows components not derived from Singleton to still emulate singleton behavior.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T Get<T>(T instance) where T : Component
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    if (ApplicationIsQuitting)
                        Debug.LogWarning("There was an attempt to create/reference Singleton<" + typeof(T) + "> while the application is quitting.");
                    else
                    {
                        var allInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
                        instance = (allInstances != null && allInstances.Length > 0 ? allInstances[0] : null);

                        if (allInstances != null && allInstances.Length > 1)
                            Debug.LogWarning("There are " + allInstances.Length + " instances of Singleton<" + typeof(T) + ">. This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");

                        if (instance == null)
                        {
                            GameObject go = new GameObject();
                            go.name = typeof(T).Name + " (Singleton)";
                            instance = go.AddComponent<T>();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// This method will return true if this newly introduced instance (thisInstance) had to be destroyed due to another instance already existing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisInstance"></param>
        /// <param name="instance"></param>
        public static bool Enforce<T>(T thisInstance, T currentInstance, out T instance, bool rename = false) where T : Component
        {
            if (thisInstance == currentInstance)
            {
                instance = currentInstance;
                return false;
            }

            if (currentInstance == null)
            {
                instance = thisInstance as T;

                if (rename)
                    instance.gameObject.name = typeof(T).Name + " (Singleton)";

                return false;
            }

            instance = currentInstance;
            Destroy(thisInstance.gameObject);
            return true;
        }

        protected override void Awake()
        {
            base.Awake();

            if (IsInstance(this))
                DontDestroy.Add(gameObject);
        }
    }
}