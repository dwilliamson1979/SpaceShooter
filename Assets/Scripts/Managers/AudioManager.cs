using com.dhcc.framework;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);

        [Header("References")]
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource fxAudioSource;

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
        }

        public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true, ulong delay = 0)
        {
            if (musicAudioSource.isPlaying)
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
}