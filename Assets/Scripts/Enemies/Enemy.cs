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

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D myCollider;
        [SerializeField] private AudioClip laserAudio;
        [SerializeField] private Projectile laserPrefab;
        [SerializeField] private Transform leftMuzzlePoint;
        [SerializeField] private Transform rightMuzzlePoint;
        [SerializeField] private AudioClip explosionAudio;

        private float nextFireTime;
        private Player player;
        private bool isDead;

        public event System.Action ReleaseToPool;

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
            SetSpawnPosition();
            StartCoroutine(FireRoutine());
        }

        private void SetSpawnPosition()
        {
            Vector3 spawnPosition = SpawnManager.Instance.GetSpawnPoint();
            spawnPosition.z = transform.position.z;
            transform.position = spawnPosition;
        }

        void Update()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            transform.Translate(Time.deltaTime * speed * -Vector3.up);

            if (!isDead && transform.position.y < BoundsManager.Instance.VerticalBoundary.x)
                SetSpawnPosition();
        }

        private IEnumerator FireRoutine()
        {
            float nextFire = 0;
            while (true)
            {
                nextFire = Random.Range(fireRateRange.x, fireRateRange.y);
                yield return new WaitForSeconds(nextFire);
                var laser1 = LaserPool.Get();// Instantiate(laserPrefab, leftMuzzlePoint.position, leftMuzzlePoint.rotation);
                laser1.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation);
                laser1.SetLayerMask(projectileLayer);
                var laser2 = LaserPool.Get();// Instantiate(laserPrefab, rightMuzzlePoint.position, rightMuzzlePoint.rotation);
                laser1.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation);
                laser1.SetLayerMask(projectileLayer);
                AudioManager.Instance.PlaySoundFx(laserAudio);
            }
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

        public void PoolOnCreate() { }

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