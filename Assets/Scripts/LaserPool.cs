using com.dhcc.pool;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    public static LaserPool Instance;

    [Header("Settings")]
    [SerializeField] private ComponentPool<Laser> pool;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static Laser Get() => Instance.pool.Get();
}