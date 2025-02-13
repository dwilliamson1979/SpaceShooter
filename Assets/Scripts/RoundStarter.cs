using UnityEngine;

public class RoundStarter : MonoBehaviour
{
    private void OnDestroy()
    {
        SpawnManager.Instance.StartSpawning();
    }
}