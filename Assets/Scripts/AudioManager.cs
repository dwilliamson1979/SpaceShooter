using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource fxAudioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySoundFx(AudioClip clip, float volume = 1f)
    {
        //fxAudioSource.clip = clip;
        fxAudioSource.PlayOneShot(clip, volume);
    }
}