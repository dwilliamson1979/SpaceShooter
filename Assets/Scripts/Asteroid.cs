using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Animator animator;

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

            animator.SetTrigger("OnExplode");
        }
    }

    public void ExplodeAnimationComplete()
    {
        SpawnManager.Instance.StartSpawning();
        Destroy(gameObject);
    }
}