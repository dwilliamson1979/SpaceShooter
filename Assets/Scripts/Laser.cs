using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float outOfBounds;

    void Start()
    {
        
    }

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