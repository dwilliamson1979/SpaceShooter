using com.dhcc.pool;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float lowerOutOfBounds;
    [SerializeField] private Vector2 fireRateRange;
    [SerializeField] private LayerMask projectileLayer;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D myCollider;    
    [SerializeField] private AudioClip laserAudio;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private Transform leftMuzzlePoint;
    [SerializeField] private Transform rightMuzzlePoint;
    [SerializeField] private AudioClip explosionAudio;

    private float nextFireTime;
    private Player player;
    private bool isDead;

    public event System.Action OnReleaseToPool;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Reset();
    }

    private void Reset()
    {
        animator.SetTrigger("OnReset");
        myCollider.enabled = true;
        isDead = false;
        MoveToRandomStartPos();
        StartCoroutine(FireRoutine());
    }

    private void MoveToRandomStartPos()
    {
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    void Update()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        transform.Translate(Time.deltaTime * speed * -Vector3.up);

        if(!isDead && transform.position.y < lowerOutOfBounds)
            MoveToRandomStartPos();
    }

    private IEnumerator FireRoutine()
    {
        float nextFire = 0;
        while (true)
        {
            nextFire = Random.Range(fireRateRange.x, fireRateRange.y);
            yield return new WaitForSeconds(nextFire);
            var laser1 = Instantiate(laserPrefab, leftMuzzlePoint.position, leftMuzzlePoint.rotation);
            laser1.SetLayerMask(projectileLayer);
            var laser2 = Instantiate(laserPrefab, rightMuzzlePoint.position, rightMuzzlePoint.rotation);
            laser2.SetLayerMask(projectileLayer);
            AudioManager.Instance.PlaySoundFx(laserAudio);
        }
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
        //else if (other.CompareTag("Laser"))
        //{
        //    var laser = other.GetComponent<Laser>();
        //    if (laser != null)
        //        laser.Damage();

        //    if (player != null)
        //        player.AddPoints(10);

        //    Die();
        //}
    }

    public void Damage()
    {
        Die();
    }

    private void Die()
    {
        isDead = true;
        myCollider.enabled = false;
        animator.SetTrigger("OnDeath");
        AudioManager.Instance.PlaySoundFx(explosionAudio);
        StopAllCoroutines();
    }

    public void DeathAnimationComplete()
    {
        OnReleaseToPool?.Invoke();
    }

    public void PoolCreate() { }

    public void PoolGet()
    {
        gameObject.SetActive(true);
        Reset();
    }

    public void PoolRelease()
    {
        gameObject.SetActive(false);
    }

    public void PoolDestroy() { }
}