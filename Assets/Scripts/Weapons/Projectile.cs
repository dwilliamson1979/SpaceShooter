using com.dhcc.framework;
using System;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Projectile : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private Vector2 outOfBounds;
        [SerializeField] private LayerMask layerMask;

        protected Player player;

        public event Action ReleaseToPool;

        private void Start()
        {
            GameObject playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
                player = playerGO.GetComponent<Player>();
        }

        void Update()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.up);

            if (transform.position.y > outOfBounds.y || transform.position.y < outOfBounds.x)
                Kill();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO Visitor pattern candidate?

            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                if (player != null)
                    PlayerHit(player);
            }
            else if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                    EnemyHit(enemy);
            }
            else if (other.CompareTag("Asteroid"))
            {
                var asteroid = other.GetComponent<Asteroid>();
                if (asteroid != null)
                    AsteroidHit(asteroid);
            }
        }

        protected virtual void PlayerHit(Player player)
        {
            player.TakeDamage(EDamageType.Damage, 1);
            Kill();
        }

        protected virtual void EnemyHit(Enemy enemy)
        {
            enemy.Damage();

            Kill();
        }

        protected virtual void AsteroidHit(Asteroid asteroid)
        {
            asteroid.Damage();

            Kill();
        }

        public void Damage()
        {
            Kill();
        }

        protected void Kill()
        {
            ReleaseToPool?.Invoke();
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
                if ((1 << i) == layerMask.value)
                {
                    gameObject.layer = i;
                    return;
                }
            }
        }

        public void PoolOnCreate() { }

        public void PoolOnGet()
        {
            gameObject.SetActive(true);
        }

        public void PoolOnRelease()
        {
            gameObject.SetActive(false);
        }

        public void PoolOnDestroy() { }
    }
}