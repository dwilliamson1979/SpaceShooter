using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Vector2 spawnRangeX;
    [SerializeField] private Vector2 spawnRangeY;
    [SerializeField] private float spawnInterval;

    public static SpawnManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartSpawning();
    }

    private IEnumerator Spawn()
    {
        var wfs = new WaitForSeconds(spawnInterval);
        while(true)
        {
            yield return wfs;

            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
            Instantiate(prefab, new Vector3(randomX, randomY, 0f), Quaternion.identity, enemyContainer);
        }
    }
    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}