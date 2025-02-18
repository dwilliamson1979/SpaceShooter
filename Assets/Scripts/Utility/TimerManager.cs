using com.dhcc.pool;
using com.dhcc.utility;
using System;
using System.Collections;
using UnityEngine;

namespace com.dhcc.utility
{
    public class TimerManager : MonoBehaviour
    {
        private static TimerManager instance;
        public static TimerManager Instance => SingletonEmulator.Get(instance);

        private event Action<float> onTick = delegate { };
        /// <summary>
        /// Any object that needs to be able to tick can subscribe to this event.
        /// </summary>
        static public event Action<float> OnTick
        {
            add => Instance.onTick += value;
            remove => Instance.onTick -= value;
        }

        private void Awake()
        {
            if (SingletonEmulator.Enforce(this, instance, out instance)) return;

            DontDestroy.Add(gameObject);
        }

        void Update()
        {
            onTick.Invoke(Time.deltaTime);
        }

        public static Coroutine SetTimer(System.Action callback, float interval, bool loop = false, float firstDelay = 0f)
        {
            if (Instance == null) return null;

            return Instance.StartCoroutine(SetTimerInternal(callback, interval, loop, firstDelay));
        }

        private static IEnumerator SetTimerInternal(System.Action callback, float interval, bool loop, float firstDelay)
        {
            if (!loop)
            {
                yield return new WaitForSeconds(interval);
                callback?.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(firstDelay);
                callback?.Invoke();

                WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
                while (true)
                {
                    yield return waitForSeconds;
                    callback?.Invoke();
                }
            }
        }

        public static Coroutine SetTimer<T>(System.Action<T> callback, T args, float interval, bool loop = false, float firstDelay = 0f)
        {
            if (Instance == null) return null;

            return Instance.StartCoroutine(SetTimerInternal(callback, args, interval, loop, firstDelay));
        }

        private static IEnumerator SetTimerInternal<T>(System.Action<T> callback, T args, float interval, bool loop, float firstDelay)
        {
            if (!loop)
            {
                yield return new WaitForSeconds(interval);
                callback?.Invoke(args);
            }
            else
            {
                yield return new WaitForSeconds(firstDelay);
                callback?.Invoke(args);

                WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
                while (true)
                {
                    yield return waitForSeconds;
                    callback?.Invoke(args);
                }
            }
        }

        public static void ClearTimer(Coroutine timer)
        {
            if (Instance != null)
                Instance.StopCoroutine(timer);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}