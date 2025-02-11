using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float outOfBounds;

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up);

        if (transform.position.y > outOfBounds)
            Kill();
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