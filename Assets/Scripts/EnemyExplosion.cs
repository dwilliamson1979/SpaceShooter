using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public void AnimationComplete()
    {
        Destroy(gameObject);
    }
}