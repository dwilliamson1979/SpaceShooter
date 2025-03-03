using com.dhcc.framework;
using System.Collections;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private int pointValue;
        [SerializeField] private float baseSpeed;
        [SerializeField] private Vector2 fireRateRange;
        [SerializeField] private LayerMask projectileLayer;
        [SerializeField] private bool hasSineMovement;
        [SerializeField] private float sineMovementAmplitude;
        [SerializeField] private float sineMovementFrequency;
        [SerializeField] private int startinghealth;
        [SerializeField] private int startingShield;
        [SerializeField] private bool isAggressive;
        [SerializeField] private float aggressionSpeedModifier;
        [SerializeField] private float aggressionDistance;

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D myCollider;
        [SerializeField] private AudioClip laserAudio;
        [SerializeField] private Projectile laserPrefab;
        [SerializeField] private Transform leftMuzzlePoint;
        [SerializeField] private Transform rightMuzzlePoint;
        [SerializeField] private AudioClip explosionAudio;
        [SerializeField] private GameObject shieldSprite;

        private Player player;
        private bool isDead;
        private AsyncTimer fireTimer;

        private float sineCenterX;
        private float currentSpeed;

        private bool isInitialized;

        public event System.Action ReleaseToPool;

        private DamageComp damageComp;
        private HealthComp healthComp;
        private ShieldComp shieldComp;

        private void Awake()
        {
            damageComp = GetComponent<DamageComp>();
            healthComp = GetComponent<HealthComp>();
            shieldComp = GetComponent<ShieldComp>();
        }

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

            healthComp.OnHealthChanged += OnHealthChanged;
            shieldComp.OnShieldChanged += OnShieldChanged;
        }

        private void Reset()
        {
            animator.SetTrigger("OnReset");
            myCollider.enabled = true;
            isDead = false;
            SetSpawnPosition();
            fireTimer.Start();

            healthComp.TakeDamage(EDamageType.Health, startinghealth);
            shieldComp.TakeDamage(EDamageType.Shield, startingShield);
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
            Vector3 movementDirection = -Vector3.up;

            if (hasSineMovement)
                movementDirection.x = sineMovementAmplitude * Mathf.Sin(Time.time * sineMovementFrequency);

            if (isAggressive && (player.transform.position - transform.position).magnitude <= aggressionDistance)
                currentSpeed = baseSpeed * (1f + aggressionSpeedModifier);
            else
                currentSpeed = baseSpeed;

            transform.Translate(Time.deltaTime * currentSpeed * movementDirection);

            //{
            //    Vector3 movement = new()
            //    {
            //        x = sineMovementAmplitude * Mathf.Sin(Time.time * sineMovementFrequency),
            //        y = -1f,
            //        z = 0f
            //    };

            //    transform.Translate(Time.deltaTime * speed * movement);
            //    //transform.Translate(Time.deltaTime * speed * -Vector3.up);
            //}
            //else
            //    transform.Translate(Time.deltaTime * speed * -Vector3.up);

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

        public int TakeDamage(EDamageType damageType, int amount)
        {
            return damageComp.TakeDamage(damageType, amount);
        }

        private void OnHealthChanged(int delta, HealthComp healthComp)
        {
            if (healthComp.Health.CurrentValue <= 0)
            {
                GameEvents.AddPoints.Raise(pointValue);
                Die();
            }
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

        private void OnShieldChanged(int delta, ShieldComp shieldComp)
        {
            if (shieldComp.Shield.CurrentValue > 0)
            {
                shieldSprite.SetActive(true);

                var sr = shieldSprite.GetComponent<SpriteRenderer>();
                Color color = sr.color;
                color.a = shieldComp.Shield.Percentage;
                sr.color = color;
            }
            else
                shieldSprite.SetActive(false);
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