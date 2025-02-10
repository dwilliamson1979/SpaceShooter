using com.dhcc.pool;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;
    [SerializeField] private AudioClip powerupAudio;
    [SerializeField] private Collider2D myCollider;

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
                Pickup(player);

            myCollider.enabled = false;
            AudioManager.Instance.PlaySoundFx(powerupAudio);
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }

    protected abstract void Pickup(Player player);
}