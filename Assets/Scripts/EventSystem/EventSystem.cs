using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.dhcc.eventsystem
{
    public class EventSystem : MonoBehaviour
    {
        // NOTE: By allowing a single Type to have an EventBus(T) or an EventBus, am I potentially creating confusion?
        private readonly Dictionary<Type, IEventBus> paramEventBuses = new();
        private readonly Dictionary<Type, IEventBus> noParamEventBuses = new();

        public void Subscribe<T>(Action listener) where T : IEvent
        {
            EventBus eventBus;

            if (!noParamEventBuses.ContainsKey(typeof(T)))
            {
                eventBus = new EventBus();
                noParamEventBuses[typeof(T)] = eventBus;
            }
            else
                eventBus = noParamEventBuses[typeof(T)] as EventBus;

            eventBus.Subscribe(listener);
        }

        public void Unsubscribe<T>(Action listener) where T : IEvent
        {
            if (noParamEventBuses.ContainsKey(typeof(T)))
                (noParamEventBuses[typeof(T)] as EventBus).Unsubscribe(listener);
        }

        public void Raise<T>() where T : IEvent
        {
            if (noParamEventBuses.ContainsKey(typeof(T)))
                (noParamEventBuses[typeof(T)] as EventBus).Raise();
        }

        public void Subscribe<T>(Action<T> binding) where T : IEvent
        {
            EventBus<T> eventBus;

            if (!paramEventBuses.ContainsKey(typeof(T)))
            {
                eventBus = new EventBus<T>();
                paramEventBuses[typeof(T)] = eventBus;
            }
            else
                eventBus = paramEventBuses[typeof(T)] as EventBus<T>;

            eventBus.Subscribe(binding);
        }

        public void Unsubscribe<T>(Action<T> binding) where T : IEvent
        {
            if (paramEventBuses.ContainsKey(typeof(T)))
                (paramEventBuses[typeof(T)] as EventBus<T>).Unsubscribe(binding);
        }

        public void Raise<T>(T @event) where T : IEvent
        {
            if (paramEventBuses.ContainsKey(typeof(T)))
                (paramEventBuses[typeof(T)] as EventBus<T>).Raise(@event);
        }
    }
}