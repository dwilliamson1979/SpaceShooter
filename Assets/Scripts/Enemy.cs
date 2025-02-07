using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerBounds;

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * speed);

        if(transform.position.y < lowerBounds)
            Spawn();
    }

    private void Spawn()
    {
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if(player != null)
                player.Damage();

            Destroy(gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            var laser = other.GetComponent<Laser>();
            if(laser != null)
                laser.Damage();

            Destroy(gameObject);
        }
    }
}