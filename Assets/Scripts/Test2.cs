using UnityEngine;
using com.dhcc.eventsystem;

public class Test2 : MonoBehaviour
{
    void Start()
    {
        var eventSystem = gameObject.GetOrAdd<EventSystem>();
        eventSystem.Subscribe<TestEvent1>(DoThis);
    }

    public void DoThis(TestEvent1 te)
    {
        Debug.Log($"Test2.DoThis: {te.a}");
    }
}