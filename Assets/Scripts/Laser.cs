using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private int upperBounds;

    [SerializeField] private float lifetime;
    private float timeAlive;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if(transform.position.y > upperBounds)
            Destroy(gameObject);

        //timeAlive += Time.deltaTime;
        //if(timeAlive >= lifetime)
        //    Destroy(gameObject);
    }

    public void Damage()
    {
        Destroy(gameObject);
    }
}