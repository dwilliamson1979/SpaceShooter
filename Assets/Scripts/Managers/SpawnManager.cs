using System.Collections;
using UnityEngine;
using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager instance;
        public static SpawnManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [Header("References")]
        [SerializeField] private Transform[] powerupPrefabs;
        [SerializeField] private Transform container;

        [Header("Settings")]
        [SerializeField] private Vector2 spawnRangeX;
        [SerializeField] private Vector2 spawnRangeY;
        [SerializeField] private float spawnInterval;
        [SerializeField] private ComponentPool<Enemy> enemyPool;

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }

        private void OnEnable()
        {
            GameEvents.StartRound.Subscribe(HandleStartRound);
            GameEvents.GameOver.Subscribe(HandleGameOver);
        }

        private void OnDisable()
        {
            GameEvents.StartRound.Unsubscribe(HandleStartRound);
            GameEvents.GameOver.Unsubscribe(HandleGameOver);
        }

        private IEnumerator SpawnEnemyRoutine()
        {
            WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
            while (true)
            {
                yield return wfs;

                float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
                float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
                var enemy = enemyPool.Get();
                enemy.transform.SetPositionAndRotation(new Vector3(randomX, randomY, 0f), Quaternion.identity);
            }
        }

        private IEnumerator SpawnPowerupRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(3f, 8f));

                float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
                float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
                Instantiate(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], new Vector3(randomX, randomY, 0f), Quaternion.identity, container);
            }
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
        }

        public void StopSpawning()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            StopSpawning();
        }

        public Vector3 GetSpawnPoint()
        {
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
            return new Vector3(randomX, randomY, transform.position.z);
        }

        private void HandleStartRound()
        {
            StartSpawning();
        }

        private void HandleGameOver()
        {
            StopSpawning();
        }
    }
}