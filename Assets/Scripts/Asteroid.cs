using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed;

    [Header("References")]
    [SerializeField] private GameObject explosionPrefab;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            var laser = other.GetComponent<Laser>();
            if (laser != null)
                laser.Damage();

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SpawnManager.Instance.StartSpawning();
            Destroy(gameObject);
        }
    }
}