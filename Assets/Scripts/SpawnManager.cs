using System.Collections;
using UnityEngine;
using com.dhcc.pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

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
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
        while(true)
        {
            yield return wfs;

            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
            //Instantiate(prefab, new Vector3(randomX, randomY, 0f), Quaternion.identity, enemyContainer);
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
}