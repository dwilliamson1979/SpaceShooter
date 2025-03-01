using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.dhcc.framework
{
    /// <summary>
    /// This event system uses T as a strongly name event (ex. public class PlayerDied : IGameEvent {}) while using the GameEvent class as the actual backend event.
    /// It is meant to be used locally on a GameObject. Not sure if this class will remain (maybe replaced).
    /// </summary>
    public class EventSystem : MonoBehaviour
    {
        // NOTE: By allowing a single Type to have a GameEvent(T) or a GameEvent, am I potentially creating confusion?
        private readonly Dictionary<Type, IGameEvent> paramEvents = new();
        private readonly Dictionary<Type, GameEvent> noParamEvents = new();

        public void Subscribe<T>(Action listener) where T : IGameEvent
        {
            GameEvent gameEvent;

            if (!noParamEvents.ContainsKey(typeof(T)))
            {
                gameEvent = new();
                noParamEvents[typeof(T)] = gameEvent;
            }
            else
                gameEvent = noParamEvents[typeof(T)];

            gameEvent.Subscribe(listener);
        }

        public void Unsubscribe<T>(Action listener) where T : IGameEvent
        {
            if (noParamEvents.ContainsKey(typeof(T)))
                noParamEvents[typeof(T)].Unsubscribe(listener);
        }

        public void Raise<T>() where T : IGameEvent
        {
            if (noParamEvents.ContainsKey(typeof(T)))
                noParamEvents[typeof(T)].Raise();
        }

        public void Subscribe<T>(Action<T> binding) where T : IGameEvent
        {
            GameEvent<T> gameEvent;

            if (!paramEvents.ContainsKey(typeof(T)))
            {
                gameEvent = new GameEvent<T>();
                paramEvents[typeof(T)] = gameEvent;
            }
            else
                gameEvent = paramEvents[typeof(T)] as GameEvent<T>;

            gameEvent.Subscribe(binding);
        }

        public void Unsubscribe<T>(Action<T> binding) where T : IGameEvent
        {
            if (paramEvents.ContainsKey(typeof(T)))
                (paramEvents[typeof(T)] as GameEvent<T>).Unsubscribe(binding);
        }

        public void Raise<T>(T @event) where T : IGameEvent
        {
            if (paramEvents.ContainsKey(typeof(T)))
                (paramEvents[typeof(T)] as GameEvent<T>).Raise(@event);
        }
    }
}