//using com.dhcc.pool;
//using com.dhcc.utility;
using com.dhcc.eventsystem;
using UnityEngine;

public class TestEvent1 : IEvent
{
    public int a;
}

public class TestEvent2 : IEvent
{
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
        var eventSystem = gameObject.GetOrAdd<EventSystem>();
        eventSystem.Subscribe<TestEvent1>(DoThis);
        eventSystem.Subscribe<TestEvent2>(DoThis2);
        eventSystem.Raise(new TestEvent1() { a = 335566 });
        eventSystem.Raise<TestEvent1>();
        eventSystem.Raise<TestEvent2>();

        GameEvents.StartGame.Subscribe(StartGame);
        GameEvents.StartGame.Raise();
    }

    public void DoThis(TestEvent1 te)
    {
        Debug.Log($"Test.DoThis: {te.a}");
    }

    public void DoThis2()
    {
        Debug.Log($"Test.DoThis2");
    }

    public void StartGame()
    {
        Debug.Log($"Test.StartGame");
    }




    //public int DamagePriority => throw new System.NotImplementedException();

    //public int TakeDamage(EDamageType damageType, int amount)
    //{
    //    throw new System.NotImplementedException();
    //}
}