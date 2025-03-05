using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Asteroid : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField] private int startinghealth;
        [SerializeField] private int pointValue;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Color explosionColor;

        [Header("References")]
        [SerializeField] private Explosion explosionPrefab;

        private HealthComp healthComp;

        private void Awake()
        {
            healthComp = GetComponent<HealthComp>();
        }

        void Start()
        {
            healthComp.OnHealthChanged += HandleHealthChanged;
            healthComp.Health.CurrentValue = startinghealth;
        }

        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        }

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (other.CompareTag("Laser"))
        //    {
        //        var laser = other.GetComponent<Projectile>();
        //        if (laser != null)
        //            laser.Damage();


        //    }
        //}

        public int TakeDamage(EDamageType damageType, int amount)
        {
            return healthComp.TakeDamage(damageType, amount);
        }

        private void HandleHealthChanged(int delta, HealthComp healthComp)
        {
            if (healthComp.Health.CurrentValue <= 0)
            {
                GameEvents.AddPoints.Raise(pointValue);
                Kill();
            }
        }

        public void Kill()
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.SetColor(explosionColor);
            explosion.Explode();

            Destroy(gameObject);
        }
    }
}