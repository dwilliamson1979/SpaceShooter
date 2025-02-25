using System;

namespace com.dhcc.eventsystem
{
    internal interface IEventBus { }
    internal interface IEventBus<T> : IEventBus where T : IEvent
    {
        void Subscribe(Action<T> action);
        void Unsubscribe(Action<T> action);

        void Raise(T @event);
    }
}