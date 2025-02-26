using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class RoundStarter : MonoBehaviour
    {
        private void OnDestroy()
        {
            if (!GameManager.ApplicationIsQuitting)
                SpawnManager.Instance.StartSpawning();
        }
    }
}