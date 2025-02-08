using com.dhcc.pool;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;

    private IPool pool;

    void Start()
    {
        MoveToRandomStartPos();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * -Vector3.up);

        if(transform.position.y < lowerOutOfBounds)
            MoveToRandomStartPos();
    }

    private void MoveToRandomStartPos()
    {
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();

            Die();
        }
        else if (other.CompareTag("Laser"))
        {
            var laser = other.GetComponent<Laser>();
            if (laser != null)
                laser.Damage();

            Die();
        }
    }

    private void Die()
    {
        //TODO Death effects.
        pool.Release(this);
    }

    public void PoolCreate(IPool pool)
    {
        this.pool = pool;
    }

    public void CheckOut()
    {
        gameObject.SetActive(true);
    }

    public void CheckIn()
    {
        gameObject.SetActive(false);
    }

    public void PoolDestroy()
    {
        
    }
}