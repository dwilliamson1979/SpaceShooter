using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float outOfBounds;

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up);

        if (transform.position.y < outOfBounds)
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
    }

    public void Damage()
    {
        Kill();
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}