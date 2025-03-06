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

        public int TakeDamage(EDamageType damageType, int amount)
        {
            return healthComp.TakeDamage(damageType, amount);
        }

        private void HandleHealthChanged(int delta, HealthComp healthComp)
        {
            if (healthComp.Health.CurrentValue <= 0)
            {
                GameEvents.AddPoints.Raise(pointValue);
                Explode();
            }
        }

        public void Explode()
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.SetColor(explosionColor);
            explosion.Explode();

            Destroy(gameObject);
        }
    }
}