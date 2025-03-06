using com.dhcc.framework;
using System;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class HomingMissle : Projectile
    {
        [Header("Settings")]
        [SerializeField] private float homingTTL;

        private Transform homingTarget;
        private AsyncTimer homingTTLTimer;

        private void Start()
        {
            homingTTLTimer = new(SelfDetonate, homingTTL);
        }

        protected override void ProcessMovement()
        {
            if (homingTarget != null && homingTarget.gameObject.activeInHierarchy)
            {
                Vector3 moveDirection = homingTarget.position - transform.position;
                moveDirection.Normalize();
                transform.Translate(Time.deltaTime * speed * moveDirection);
            }

            if (BoundsManager.Instance.IsOutOfBounds(transform))
                ProcessOutOfBounds();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damageType, baseDamage);

            homingTTLTimer.Stop();
            ReleaseToPool();
        }

        protected override void ProcessOutOfBounds()
        {
            homingTTLTimer.Stop();
            ReleaseToPool();
        }

        private void FindNearestEnemy()
        {
            float nearestDistance = float.MaxValue;
            var hits = Physics2D.CircleCastAll(transform.position, 10f, Vector2.up);
            foreach (var hit in hits)
            {
                float distance = Mathf.Abs(hit.distance);
                if (distance < nearestDistance && hit.collider.CompareTag("Enemy"))
                {
                    nearestDistance = distance;
                    homingTarget = hit.transform;
                }
            }
        }

        void SelfDetonate()
        {
            if (gameObject.activeInHierarchy)
            {
                homingTTLTimer.Stop();
                ReleaseToPool();
            }
        }

        public override void PoolOnGet()
        {
            FindNearestEnemy();
            base.PoolOnGet();
        }
    }
}