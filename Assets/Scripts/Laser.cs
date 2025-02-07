using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float upperOutOfBounds;

    [SerializeField] private float lifetime;
    private float timeAlive;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if(transform.position.y > upperOutOfBounds)
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