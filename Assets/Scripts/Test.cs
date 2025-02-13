using com.dhcc.pool;
using com.dhcc.utility;
using UnityEngine;

public class Test : MonoBehaviour
{
    private static Test instance;
    public static Test Instance => SingletonEmulator.Get(instance);

    private void Awake()
    {
        if (SingletonEmulator.Enforce(this, instance, out instance)) return;
    }

    void Start()
    {
        Debug.Log("Test.Start()");
    }
}