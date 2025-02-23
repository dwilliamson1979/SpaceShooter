//using com.dhcc.pool;
//using com.dhcc.utility;
using UnityEngine;

public class Test : MonoBehaviour, IDamageReceiver
{
    //private static Test instance;
    //public static Test Instance => SingletonEmulator.Get(instance);

    //private void Awake()
    //{
    //    if (SingletonEmulator.Enforce(this, instance, out instance)) return;
    //}

    //void Start()
    //{
    //    Debug.Log("Test.Start()");
    //}
    public int DamagePriority => throw new System.NotImplementedException();

    public int TakeDamage(EDamageType damageType, int amount)
    {
        throw new System.NotImplementedException();
    }
}