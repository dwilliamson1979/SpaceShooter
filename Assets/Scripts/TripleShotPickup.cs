using com.dhcc.pool;
using UnityEngine;

public class TripleShotPickup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;

    void Start()
    {
        MoveToRandomStartPos();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * -Vector3.up);

        if (transform.position.y < lowerOutOfBounds)
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
                player.ActivateTripleShot();

            Destroy(gameObject);
        }
    }
}