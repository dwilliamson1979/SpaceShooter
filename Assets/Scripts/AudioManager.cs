using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("References")]
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

    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true, ulong delay = 0)
    {
        if(musicAudioSource.isPlaying)
            musicAudioSource.Stop();

        musicAudioSource.clip = clip;
        musicAudioSource.volume = volume;
        musicAudioSource.loop = loop;
        musicAudioSource.Play(delay);
    }

    public void PlaySoundFx(AudioClip clip, float volume = 1f)
    {
        fxAudioSource.PlayOneShot(clip, volume);
    }
}