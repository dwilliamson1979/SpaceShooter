using System;

namespace com.dhcc.framework
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

    public class GameEvent<T>
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

    public class GameEvent<T1, T2>
    {
        private event Action<T1, T2> onEvent = null;
        public event Action<T1, T2> OnEvent
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

        public void Subscribe(Action<T1, T2> subscriber) => OnEvent += subscriber;
        public void Unsubscribe(Action<T1, T2> subscriber) => OnEvent -= subscriber;

        public void Raise(T1 arg1, T2 arg2) => onEvent?.Invoke(arg1, arg2);

        ~GameEvent() => onEvent = null;
    }
}