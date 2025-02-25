using UnityEngine;
using com.dhcc.eventsystem;

public class Test2 : MonoBehaviour
{
    void Start()
    {
        var bus = gameObject.GetOrAdd<LocalEventBus>();
        bus.EventSystem.Subscribe<TestEvent>(DoThis);
    }

    public void DoThis(TestEvent te)
    {
        Debug.Log($"Test2.DoThis: {te.a}");
    }
}