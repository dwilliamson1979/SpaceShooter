using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
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

            //animator.SetTrigger("OnExplode");
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SpawnManager.Instance.StartSpawning();
            Destroy(gameObject);
        }
    }

    //public void ExplodeAnimationComplete()
    //{
    //    SpawnManager.Instance.StartSpawning();
    //    Destroy(gameObject);
    //}
}