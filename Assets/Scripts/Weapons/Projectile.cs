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
        [SerializeField] private bool isHoming;
        [SerializeField] private float homingTTL;

        protected Player player;

        private Transform homingTarget;
        private AsyncTimer homingTTLTimer;

        public event Action ReleaseToPool;

        private void Start()
        {
            GameObject playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
                player = playerGO.GetComponent<Player>();

            homingTTLTimer = new(Dummy, homingTTL);
        }

        void Dummy()
        {
            Debug.Log("Dummy");

            if(gameObject.activeInHierarchy)
                Kill();
        }

        void Update()
        {
            if(isHoming && homingTarget != null)
            {
                Vector3 moveDirection = homingTarget.position - transform.position;
                moveDirection.Normalize();
                transform.Translate(Time.deltaTime * speed * moveDirection);
            }
            else
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
            enemy.TakeDamage(EDamageType.Damage, 1);

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
            homingTTLTimer.Stop();
            EnableHoming(false);
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

        public void EnableHoming(bool enabled)
        {
            isHoming = enabled;

            if (isHoming)
            {
                homingTTLTimer.Start();
                FindNearestEnemy();
            }
            else
            {
                homingTTLTimer.Stop();
            }
        }

        private void FindNearestEnemy()
        {
            //float nearestDistance = float.MaxValue;
            //for (int i = 0; i < SpawnManager.Instance.EnemyPool.Container.childCount; i++)
            //{
            //    Transform enemyTransform = SpawnManager.Instance.EnemyPool.Container.GetChild(i);
            //    float distance = MathF.Abs((enemyTransform.position - transform.position).magnitude);
            //    if (enemyTransform.gameObject.activeInHierarchy && nearestDistance > distance)
            //    {
            //        nearestDistance = distance;
            //        homingTarget = enemyTransform;
            //    }
            //}

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

            //var hits = Physics2D.CircleCastAll(transform.position, 10f, Vector2.up);
            //foreach(var hit in hits)
            //    Debug.Log(hit.collider.gameObject.ToString());
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