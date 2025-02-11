using com.dhcc.pool;
using System;
using UnityEngine;

public class Laser : MonoBehaviour, IPoolObject
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float outOfBounds;
    [SerializeField] private LayerMask layerMask;

    private Player player;

    public event Action OnReleaseToPool;

    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
            player = playerGO.GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up);

        if (transform.position.y > outOfBounds)
            Kill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();

            Kill();
        }
        else if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.Damage();

            if (player != null)
                player.AddPoints(10);

            Kill();
        }
    }

    public void Damage()
    {
        Kill();
    }

    private void Kill()
    {
        //Destroy(gameObject);
        OnReleaseToPool?.Invoke();
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        if(layerMask.value == 0)
        {
            gameObject.layer = 0;
            return;
        }

        for (int i = 0; i <= 31; i++)
        {
            if ((1 << i) == layerMask.value)
            {
                gameObject.layer = i;
                return;
            }
        }
    }

    public void PoolCreate() { }

    public void PoolGet()
    {
        gameObject.SetActive(true);
    }

    public void PoolRelease()
    {
        gameObject.SetActive(false);
    }

    public void PoolDestroy() { }
}