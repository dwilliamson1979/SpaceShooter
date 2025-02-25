using System.Collections.Generic;
using System;
using UnityEngine;
using static com.dhcc.eventsystem.EventSystem;
using UnityEngine.UIElements;

namespace com.dhcc.eventsystem
{
    public interface IEventBus { }
    public interface IEventBus<T> : IEventBus where T : IEvent
    {
        void Subscribe(Action<T> action);
        void Unsubscribe(Action<T> action);

        void Raise(T @event);
    }

    public class EventBus<T> : IEventBus<T> where T : IEvent
    {
        private readonly HashSet<Action<T>> bindings = new();

        public void Subscribe(Action<T> binding) => bindings.Add(binding);
        public void Unsubscribe(Action<T> binding) => bindings.Remove(binding);

        public void Raise(T @event)
        {
            foreach (var binding in bindings)
                binding.Invoke(@event);
        }
    }

    public interface IEvent { }

    public class EventSystem
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