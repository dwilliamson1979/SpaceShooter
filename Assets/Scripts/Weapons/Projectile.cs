using com.dhcc.framework;
using System;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Projectile : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] protected EDamageType damageType;
        [SerializeField] protected int baseDamage;
        [SerializeField] protected float speed;
        [SerializeField] protected LayerMask layerMask;

        public event Action PoolRelease;        

        void Update()
        {
            ProcessMovement();
        }

        protected virtual void ProcessMovement()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.up);

            if (BoundsManager.Instance.IsOutOfBounds(transform))
                ProcessOutOfBounds();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damageType, baseDamage);

            PoolRelease?.Invoke();
        }

        protected virtual void ProcessOutOfBounds()
        {
            PoolRelease?.Invoke();
        }

        public void SetLayerMask(LayerMask layerMask)
        {
            if (layerMask.value == 0)
            {
                gameObject.layer = 0;
                return;
            }

            for (int i = 0; i <= 31; i++)
            {
                if ((1 << i) == layerMask.value) //Am I missing something? Binary comparison to capture all set bits?
                {
                    gameObject.layer = i;
                    return;
                }
            }
        }

        public virtual void PoolOnCreate() { }
        public virtual void PoolOnGet() => gameObject.SetActive(true);
        public virtual void PoolOnRelease() => gameObject.SetActive(false);
        protected void ReleaseToPool() => PoolRelease?.Invoke();
        public virtual void PoolOnDestroy() { }
    }
}