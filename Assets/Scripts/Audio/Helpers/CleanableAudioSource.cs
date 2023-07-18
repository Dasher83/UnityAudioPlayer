using UnityEngine;

namespace UnityAudioPlayer.Audio.Helpers
{
    [System.Serializable]
    public class CleanableAudioSource
    {
        public AudioSource audioSource;
        public float lastStoppedTime;
    }
}
