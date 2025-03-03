using com.dhcc.framework;
using System.Collections;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private int pointValue;
        [SerializeField] private float speed;
        [SerializeField] private Vector2 fireRateRange;
        [SerializeField] private LayerMask projectileLayer;
        [SerializeField] private bool hasSineMovement;
        [SerializeField] private float sineMovementAmplitude;
        [SerializeField] private float sineMovementFrequency;

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D myCollider;
        [SerializeField] private AudioClip laserAudio;
        [SerializeField] private Projectile laserPrefab;
        [SerializeField] private Transform leftMuzzlePoint;
        [SerializeField] private Transform rightMuzzlePoint;
        [SerializeField] private AudioClip explosionAudio;

        private Player player;
        private bool isDead;
        private AsyncTimer fireTimer;

        private float sineCenterX;

        private bool isInitialized;

        public event System.Action ReleaseToPool;

        void Start()
        {
            Init();
        }

        private void Init()
        {
            if (isInitialized) return;

            isInitialized = true;
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            fireTimer = new(Fire, Random.Range(fireRateRange.x, fireRateRange.y), true);
        }

        private void Reset()
        {
            animator.SetTrigger("OnReset");
            myCollider.enabled = true;
            isDead = false;
            SetSpawnPosition();
            fireTimer.Start();
        }

        private void SetSpawnPosition()
        {
            Vector3 spawnPosition = SpawnManager.Instance.GetSpawnPoint();
            spawnPosition.z = transform.position.z;
            transform.position = spawnPosition;
            sineCenterX = spawnPosition.x;
        }

        void Update()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            if (hasSineMovement)
            {
                Vector3 movement = new()
                {
                    x = sineMovementAmplitude * Mathf.Sin(Time.time * sineMovementFrequency),
                    y = -1f,
                    z = 0f
                };
                
                transform.Translate(Time.deltaTime * speed * movement);
                //transform.Translate(Time.deltaTime * speed * -Vector3.up);
            }
            else
                transform.Translate(Time.deltaTime * speed * -Vector3.up);

            if (!isDead && transform.position.y < BoundsManager.Instance.VerticalBoundary.x)
                SetSpawnPosition();
        }

        private void Fire()
        {
            var laser1 = LaserPool.Get();// Instantiate(laserPrefab, leftMuzzlePoint.position, leftMuzzlePoint.rotation);
            laser1.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation);
            laser1.SetLayerMask(projectileLayer);
            var laser2 = LaserPool.Get();
            laser2.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation);
            laser2.SetLayerMask(projectileLayer);
            AudioManager.Instance.PlaySoundFx(laserAudio);

            //fireTimer.SetInterval(Random.Range(fireRateRange.x, fireRateRange.y));
            //fireTimer.Start();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                if (player != null)
                    player.TakeDamage(EDamageType.Damage, 1);

                Die();
            }
        }

        public void Damage()
        {
            GameEvents.AddPoints.Raise(pointValue);
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
            SetSpawnPosition();
            ReleaseToPool?.Invoke();
        }

        public void PoolOnCreate() 
        {
            Init();
        }

        public void PoolOnGet()
        {
            gameObject.SetActive(true);
            Reset();
        }

        public void PoolOnRelease()
        {
            gameObject.SetActive(false);
        }

        public void PoolOnDestroy() { }
    }
}