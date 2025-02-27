using System;

namespace com.dhcc.framework
{
    internal class EventBus : IEventBus
    {
        private event Action onEvent;

        public void Subscribe(Action listener) => onEvent += listener;
        public void Unsubscribe(Action listener) => onEvent -= listener;
        public void Raise() => onEvent.Invoke();
    }

    internal class EventBus<T> : IEventBus where T : IEvent
    {
        private event Action<T> onEvent;

        public void Subscribe(Action<T> listener) => onEvent += listener;
        public void Unsubscribe(Action<T> listener) => onEvent -= listener;
        public void Raise(T args) => onEvent.Invoke(args);
    }
}