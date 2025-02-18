using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;

    [Header("References")]
    [SerializeField] private PickupSO pickupSO;

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
            if (pickupSO != null)
                pickupSO.TryToPickup(this, other.gameObject);
        }
    }

    protected virtual void TryToPickup(GameObject obj) { }
    public virtual void PickupComplete()
    {
        Destroy(gameObject);

        //TODO Release to pool?
    }
}