using com.dhcc.pool;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D myCollider;

    public event System.Action<IPoolObject> OnReleaseToPool;

    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        MoveToRandomStartPos();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * -Vector3.up);

        if(transform.position.y < lowerOutOfBounds)
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
            var player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();

            Die();
        }
        else if (other.CompareTag("Laser"))
        {
            var laser = other.GetComponent<Laser>();
            if (laser != null)
                laser.Damage();

            if (player != null)
                player.AddPoints(10);

            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("OnEnemyDeath");
        myCollider.enabled = false;
        OnReleaseToPool?.Invoke(this);
    }

    public void PoolCreate()
    {

    }

    public void PoolGet()
    {
        gameObject.SetActive(true);
        animator.Play("ANIM_EnemyFly");
        myCollider.enabled = true;
    }

    public void PoolRelease()
    {
        gameObject.SetActive(false);
    }

    public void PoolDestroy()
    {
        
    }
}