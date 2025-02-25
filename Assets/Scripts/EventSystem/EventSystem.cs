using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.dhcc.eventsystem
{
    public class EventBus<T> : IEventBus<T> where T : IEvent
    {
        private Action<T> onEvent;

        public void Subscribe(Action<T> listener) => onEvent += listener;
        public void Unsubscribe(Action<T> listener) => onEvent -= listener;
        public void Raise(T args) => onEvent.Invoke(args);
    }

    public interface IEvent { }

    public class EventSystem : MonoBehaviour
    {
        private readonly Dictionary<Type, IEventBus> eventBuses = new();

        public void Subscribe<T>(Action<T> binding) where T : IEvent
        {
            IEventBus<T> eventBus;

            if (!eventBuses.ContainsKey(typeof(T)))
            {
                eventBus = new EventBus<T>();
                eventBuses[typeof(T)] = eventBus;
            }
            else
                eventBus = eventBuses[typeof(T)] as EventBus<T>;

            eventBus.Subscribe(binding);
        }

        public void Unsubscribe<T>(Action<T> binding) where T : IEvent
        {
            if (eventBuses.ContainsKey(typeof(T)))
                (eventBuses[typeof(T)] as IEventBus<T>).Unsubscribe(binding);
        }

        public void Raise<T>(T @event) where T : IEvent
        {
            if (eventBuses.ContainsKey(typeof(T)))
                (eventBuses[typeof(T)] as IEventBus<T>).Raise(@event);
        }
    }
}