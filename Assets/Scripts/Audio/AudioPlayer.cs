using System;
using System.Collections;
using System.Linq;
using UnityAudioPlayer.Audio.Helpers;
using UnityEngine;
using Logger = UnityAudioPlayer.Audio.Helpers.Logger;

namespace UnityAudioPlayer.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private const float DEFAULT_VOLUME = 1f;

        [Range(0f, 1f)]
        [SerializeField]
        private float masterVolume = DEFAULT_VOLUME;

        [Space]
        [Tooltip("Registry of Configurable Audio Clips to play")]
        [SerializeField]
        private AudioClipRegistry audioClipRegistry;

        private AudioSourcePool audioSourcePool;
        private Logger logger = new Logger();

        private void Start()
        {
            audioSourcePool = GetComponent<AudioSourcePool>();
        }

        public float MasterVolume
        {
            get { return masterVolume; }
            set { masterVolume = Mathf.Clamp01(value); }
        }

        public void Play(string audioClipAlias)
        {
            ConfigurableAudioClip configurableAudioClip = audioClipRegistry.GetAudioClip(audioClipAlias);

            CleanableAudioSource freeCleanableSource = audioSourcePool.GetFreeAudioSource();

            freeCleanableSource.audioSource.clip = configurableAudioClip.audioClip;
            freeCleanableSource.audioSource.volume = MasterVolume * configurableAudioClip.volume;

            if (configurableAudioClip.playsWithDelay)
            {
                StartCoroutine(PlayWithDelay(freeCleanableSource.audioSource, configurableAudioClip.delay));
            }
            else
            {
                freeCleanableSource.audioSource.Play();
            }
        }

        private IEnumerator PlayWithDelay(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            audioSource.Play();
        }

        public void Stop(string audioClipAlias)
        {
            CleanableAudioSource playingAudioSource = GetPlayingAudioSource(audioClipAlias);

            if (playingAudioSource != null)
            {
                playingAudioSource.audioSource.Stop();
                playingAudioSource.lastStoppedTime = Time.time;
            }
            else
            {
                logger.LogNoAudioClipError(audioClipAlias, state: "playing");
            }
        }

        public void StopAll()
        {
            foreach (CleanableAudioSource playingAudioSource in audioSourcePool.GetPlayingAudioSources())
            {
                playingAudioSource.audioSource.Stop();
                playingAudioSource.lastStoppedTime = Time.time;
            }
        }

        public void Pause(string audioClipAlias)
        {
            CleanableAudioSource playingSource = GetPlayingAudioSource(audioClipAlias);

            if (playingSource != null)
            {
                playingSource.audioSource.Pause();
            }
            else
            {
                logger.LogNoAudioClipError(audioClipAlias, state: "playing");
            }
        }

        public void Resume(string audioClipAlias)
        {
            CleanableAudioSource pausedSource = GetPausedAudioSource(audioClipAlias);

            if (pausedSource != null)
            {
                pausedSource.audioSource.Play();
            }
            else
            {
                logger.LogNoAudioClipError(audioClipAlias, state: "paused");
            }
        }

        private CleanableAudioSource GetPlayingAudioSource(string audioClipAlias)
        {
            ConfigurableAudioClip clip = audioClipRegistry.GetAudioClip(audioClipAlias);

            return audioSourcePool
                .GetPlayingAudioSources()
                .FirstOrDefault(source => source.audioSource.clip == clip.audioClip);
        }

        private CleanableAudioSource GetPausedAudioSource(string audioClipAlias)
        {
            ConfigurableAudioClip clip = audioClipRegistry.GetAudioClip(audioClipAlias);

            return audioSourcePool
                .GetPausedAudioSources()
                .FirstOrDefault(source => source.audioSource.clip == clip.audioClip);
        }

        public void ChangePlayingAudioClipVolume(string audioClipAlias, float newVolume)
        {
            CleanableAudioSource playingSource = GetPlayingAudioSource(audioClipAlias);

            if (playingSource != null)
            {
                playingSource.audioSource.volume = Mathf.Clamp01(newVolume);
                audioClipRegistry.ChangeClipVolume(audioClipAlias, newVolume);
            }
            else
            {
                logger.LogNoAudioClipError(audioClipAlias, state: "playing");
            }
        }
    }
}
