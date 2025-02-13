using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Color explosionColor;

    [Header("References")]
    [SerializeField] private Explosion explosionPrefab;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Laser"))
    //    {
    //        var laser = other.GetComponent<Projectile>();
    //        if (laser != null)
    //            laser.Damage();

            
    //    }
    //}

    public void Damage()
    {
        Kill();
    }

    public void Kill()
    {
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.SetColor(explosionColor);
        explosion.Explode();

        Destroy(gameObject);
    }
}