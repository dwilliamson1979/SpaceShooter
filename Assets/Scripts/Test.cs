//using com.dhcc.pool;
//using com.dhcc.utility;
using com.dhcc.eventsystem;
using UnityEngine;

public class TestEvent : IEvent
{
    public int a;
}

public class Test : MonoBehaviour//, IDamageReceiver
{
    //private static Test instance;
    //public static Test Instance => SingletonEmulator.Get(instance);

    //private void Awake()
    //{
    //    if (SingletonEmulator.Enforce(this, instance, out instance)) return;
    //}

    void Start()
    {
        var bus = gameObject.GetOrAdd<LocalEventBus>();
        bus.EventSystem.Subscribe<TestEvent>(DoThis);
        bus.EventSystem.Raise(new TestEvent() { a = 335566 });
    }

    public void DoThis(TestEvent te)
    {
        Debug.Log($"Test.DoThis: {te.a}");
    }




    //public int DamagePriority => throw new System.NotImplementedException();

    //public int TakeDamage(EDamageType damageType, int amount)
    //{
    //    throw new System.NotImplementedException();
    //}
}