using UnityEngine;

namespace Runtime.Systems
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource sfxSource;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip dragClip;
        
        public void PlaySound(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void PlayDragSound()
        {
            sfxSource.PlayOneShot(dragClip);
        }
    }
}