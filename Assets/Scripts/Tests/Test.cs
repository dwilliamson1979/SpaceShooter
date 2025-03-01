using com.dhcc.framework;
using UnityEngine;

public class TestEvent1 : IGameEvent
{
    public int a;
}

public class TestEvent2 : IGameEvent
{
}

public class Test : MonoBehaviour
{
    void Start()
    {
        var eventSystem = gameObject.GetOrAdd<EventSystem>();
        eventSystem.Subscribe<TestEvent1>(DoThis);
        eventSystem.Subscribe<TestEvent2>(DoThis2);
        eventSystem.Raise(new TestEvent1() { a = 335566 });
        eventSystem.Raise<TestEvent1>();
        eventSystem.Raise<TestEvent2>();
    }

    private void DoThis(TestEvent1 args)
    {
        Debug.Log($"DoThis {args.a}");
    }

    private void DoThis2()
    {
        Debug.Log("DoThis2");
    }
}