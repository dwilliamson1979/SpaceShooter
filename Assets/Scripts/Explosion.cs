using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip explosionAudio;

    void Start()
    {
        AudioManager.Instance.PlaySoundFx(explosionAudio);
    }

    public void AnimationComplete()
    {
        Destroy(gameObject);
    }
}