using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.dhcc.eventsystem
{
    public class GameEvent
    {

//#if UNITY_EDITOR
//        private static PlayModeStateChange PlayModeState { get; set; }

//        [InitializeOnLoadMethod]
//        public static void InitializeEditor()
//        {
//            EditorApplication.playModeStateChanged -= OnPlayStateModeChanged;
//            EditorApplication.playModeStateChanged += OnPlayStateModeChanged;
//        }

//        static void OnPlayStateModeChanged(PlayModeStateChange state)
//        {
//            PlayModeState = state;
//            if (state == PlayModeStateChange.ExitingPlayMode)
//                Reset?.Invoke();
//        }
//#endif

        private event Action onEvent = null;
        public event Action OnEvent
        {
            add 
            {
                onEvent -= value;
                onEvent += value; 
            }
            remove 
            { 
                onEvent -= value;
            }
        }

        public void Subscribe(Action subscriber) => OnEvent += subscriber;
        public void Unsubscribe(Action subscriber) => OnEvent -= subscriber;

        public void Raise() => onEvent?.Invoke();

        ~GameEvent() => onEvent = null;
    }

    public class GameEvent<T> where T : class, new()
    {
        private event Action<T> onEvent = null;
        public event Action<T> OnEvent
        {
            add
            {
                onEvent -= value;
                onEvent += value;
            }
            remove
            {
                onEvent -= value;
            }
        }

        public void Subscribe(Action<T> subscriber) => OnEvent += subscriber;
        public void Unsubscribe(Action<T> subscriber) => OnEvent -= subscriber;

        public void Raise(T args) => onEvent?.Invoke(args);

        ~GameEvent() => onEvent = null;
    }
}