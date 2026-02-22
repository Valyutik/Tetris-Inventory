using UnityEngine;

namespace Runtime.Systems
{
    public interface IAudioService
    {
        public void PlaySound(AudioClip clip);
        public void PlayDragSound();
    }
}