using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace com.dhcc.framework.ai
{
    public class Context
    {
        public Brain brain;
        public NavMeshAgent agent;
        public Transform target;
        public Sensor sensor;

        //NOTE mentioned @ 3:20 mark. Something about a blackboard-like object.
        readonly Dictionary<string, object> data = new();

        public Context(Brain brain)
        {
            this.brain = brain;
            this.agent = brain.gameObject.GetOrAdd<NavMeshAgent>();
            this.sensor = brain.gameObject.GetOrAdd<Sensor>();
        }

        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;

        //NOTE: Would an interface be a better idea?
        public void SetData(string key, object value) => data[key] = value;
    }
}