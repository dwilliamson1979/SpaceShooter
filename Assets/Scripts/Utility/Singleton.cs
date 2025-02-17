using UnityEngine;

namespace com.dhcc.utility
{
    /// <summary>
    /// A singleton class which which will create a singleton or use an instance already placed in the level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        //Objects are not destroyed in a particular order when the application quits.
        //This allows objects referencing the singleton to check before performing actions against a singleton which is being destroyed.
        public static bool ApplicationIsQuitting { get; private set; }

        private static T instance;
        private static readonly object lockObject = new object();

        public static T Instance
        {
            get
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
            private set { instance = value; }
        }

        protected static bool IsInstance(T obj) => obj == null || instance == null || instance != obj ? false : true;

        protected virtual void Awake()
        {
            if (instance == this)
                gameObject.name = typeof(T).Name + " (Singleton)";
            else if (instance == null)
            {
                Instance = this as T;
                gameObject.name = typeof(T).Name + " (Singleton)";
            }
            else
                Destroy(gameObject);
        }

        private void OnApplicationQuit()
        {
            /// <summary>
            /// THIS WAS TAKEN FROM A PHOTON EXAMPLE. NOT SURE IF IT IS STILL RELEVANT.
            /// When Unity quits, it destroys objects in a random order.
            /// In principle, a Singleton is only destroyed when application quits.
            /// If any script calls Instance after it have been destroyed, 
            /// it will create a buggy ghost object that will stay on the Editor scene
            /// even after stopping playing the Application. Really bad!
            /// So, this was made to be sure we're not creating that buggy ghost object.
            /// </summary>

            ApplicationIsQuitting = true;
        }
    }
}