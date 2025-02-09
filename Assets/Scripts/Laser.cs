using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float upperOutOfBounds;

    //[SerializeField] private float lifetime;
    //private float timeAlive;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up);

        if (transform.position.y > upperOutOfBounds)
            Kill();

        //timeAlive += Time.deltaTime;
        //if(timeAlive >= lifetime)
        //    Destroy(gameObject);
    }

    public void Damage()
    {
        Kill();
    }

    private void Kill()
    {
        //if(transform.parent != null)
        //    Destroy(transform.parent.gameObject);

        Destroy(gameObject);
    }
}