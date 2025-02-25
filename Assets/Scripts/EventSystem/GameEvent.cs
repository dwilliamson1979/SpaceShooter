using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.dhcc.eventsystem
{
    public class GameEvent
    {

#if UNITY_EDITOR
        public static PlayModeStateChange PlayModeState { get; set; }

        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayStateModeChanged;
            EditorApplication.playModeStateChanged += OnPlayStateModeChanged;
        }

        static void OnPlayStateModeChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
                Reset?.Invoke();
        }
#endif

        private static Action Reset;

        internal List<Delegate> listeners = null;

        public GameEvent() => Reset += HandleReset;

        private void HandleReset()
        {
            if (listeners != null)
                listeners.Clear();

            listeners = null;
        }

        internal void Init()
        {
            if (listeners == null)
                listeners = new List<Delegate>();
        }

        public void Subscribe(Action subscriber)
        {
            Init(); //Lazily initialize

            if (!listeners.Contains(subscriber))
                listeners.Add(subscriber);
        }

        public void Unsubscribe(Action subscriber)
        {
            if (listeners == null) return;

            if (listeners.Contains(subscriber))
                listeners.Remove(subscriber);
        }

        public void Raise()
        {
            if (listeners == null) return;

            for (int i = 0; i < listeners.Count; i++)
                listeners[i].DynamicInvoke();
        }

        ~GameEvent()
        {
            listeners = null;
            Reset -= HandleReset;
        }
    }

    public class GameEvent<T> : GameEvent where T : class, new()
    {
        public void Subscribe(Action<T> subscriber)
        {
            Init(); //Lazily initialize

            if (!listeners.Contains(subscriber))
                listeners.Add(subscriber);
        }

        public void Unsubscribe(Action<T> subscriber)
        {
            if (listeners == null) return;

            if (listeners.Contains(subscriber))
                listeners.Remove(subscriber);
        }

        public void Raise(T args)
        {
            if (listeners == null) return;

            for (int i = 0; i < listeners.Count; i++)
                listeners[i].DynamicInvoke(args);
        }
    }
}