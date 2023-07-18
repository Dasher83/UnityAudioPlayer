using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityAudioPlayer.Audio.Helpers
{
    public class AudioSourcePool : MonoBehaviour
    {
        private const int INITIAL_AUDIO_SOURCE_COUNT = 2;
        private const float AUDIO_SOURCE_IDLE_SECONDS = 60f;
        private const float CLEANUP_INTERVAL_SECONDS = 30f;

        [Tooltip("Configuration for the Audio Sources")]
        [SerializeField] private AudioSourceConfig audioSourceConfig;

        private List<CleanableAudioSource> cleanableAudioSources;

        private void Awake()
        {
            cleanableAudioSources = new List<CleanableAudioSource>();

            for (int i = 0; i < INITIAL_AUDIO_SOURCE_COUNT; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                ConfigureAudioSource(audioSource);
                cleanableAudioSources.Add(
                    new CleanableAudioSource
                    {
                        audioSource = audioSource,
                        lastStoppedTime = Time.time
                    });
            }
        }

        private void Start()
        {
            StartCoroutine(CleanupCoroutine());
        }

        private void ConfigureAudioSource(AudioSource audioSource)
        {
            audioSource.playOnAwake = audioSourceConfig.playOnAwake;
            audioSource.loop = audioSourceConfig.loop;
            audioSource.rolloffMode = audioSourceConfig.rolloffMode;
            audioSource.minDistance = audioSourceConfig.minDistance;
            audioSource.maxDistance = audioSourceConfig.maxDistance;
            audioSource.outputAudioMixerGroup = audioSourceConfig.outputAudioMixerGroup;
            // Apply more configuration options as needed
        }

        public CleanableAudioSource GetFreeAudioSource()
        {
            CleanableAudioSource freeCleanableSource = cleanableAudioSources
                .FirstOrDefault(cleanableSource => cleanableSource.audioSource.isPlaying == false);

            if (freeCleanableSource == null)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                ConfigureAudioSource(audioSource);
                freeCleanableSource = new CleanableAudioSource { audioSource = audioSource, lastStoppedTime = Time.time };
                cleanableAudioSources.Add(freeCleanableSource);
            }

            return freeCleanableSource;
        }

        public IEnumerable<CleanableAudioSource> GetPlayingAudioSources()
        {
            return cleanableAudioSources.Where(cleanableAudioSource => cleanableAudioSource.audioSource.isPlaying);
        }

        public IEnumerable<CleanableAudioSource> GetPausedAudioSources()
        {
            return cleanableAudioSources.Where(cleanableAudioSource => cleanableAudioSource.audioSource.isPlaying == false);
        }

        private void Cleanup()
        {
            for (int i = cleanableAudioSources.Count - 1; i >= 0; i--)
            {
                CleanableAudioSource audioSource = cleanableAudioSources[i];

                if (!audioSource.audioSource.isPlaying && Time.time - audioSource.lastStoppedTime > AUDIO_SOURCE_IDLE_SECONDS)
                {
                    Destroy(audioSource.audioSource);
                    cleanableAudioSources.RemoveAt(i);
                }
            }
        }

        private IEnumerator CleanupCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(CLEANUP_INTERVAL_SECONDS);
                Cleanup();
            }
        }
    }
}
