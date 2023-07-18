using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private const int INITIAL_AUDIO_SOURCE_COUNT = 2;
    private const float DEFAULT_VOLUME = 1f;
    private const float AUDIO_SOURCE_IDLE_SECONDS = 60f;
    private const float CLEANUP_INTERVAL_SECONDS = 30f;

    private const string NO_AUDIO_CLIP_ERROR_TEMPLATE = "No AudioClip found {0} with the alias or clip name: ";
    private const string MULTIPLE_AUDIO_CLIPS_ERROR_TEMPLATE = "Multiple ConfigurableAudioClips found with the same alias: ";

    [Tooltip("Configurable Audio Clips to play")]
    [SerializeField]
    private ConfigurableAudioClip[] audioClips;

    [Space]
    [Range(0f, 1f)]
    [SerializeField]
    private float volume = DEFAULT_VOLUME;

    [Space]
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

    public float Volume
    {
        get { return volume; }
        set { volume = Mathf.Clamp01(value); }
    }

    public void Play(string audioClipAlias)
    {
        if (string.IsNullOrEmpty(audioClipAlias))
        {
            Debug.LogError("No alias provided. Cannot play AudioClip.");
            return;
        }

        ConfigurableAudioClip configAudioClip = audioClips.FirstOrDefault(clip => clip.alias == audioClipAlias);

        if (configAudioClip == null)
        {
            configAudioClip = audioClips.FirstOrDefault(clip => clip.audioClip.name == audioClipAlias);

            if (configAudioClip == null)
            {
                Debug.LogError("No ConfigurableAudioClip found with the alias or clip name: " + audioClipAlias);
                return;
            }
        }

        if (audioClips.Count(clip => clip.alias == audioClipAlias) > 1)
        {
            LogMultipleAudioClipsError(audioClipAlias);
            return;
        }

        CleanableAudioSource freeCleanableSource = cleanableAudioSources
            .FirstOrDefault(cleanableSource => cleanableSource.audioSource.isPlaying == false);

        if (freeCleanableSource == null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            ConfigureAudioSource(audioSource);
            freeCleanableSource = new CleanableAudioSource { audioSource = audioSource, lastStoppedTime = Time.time };
            cleanableAudioSources.Add(freeCleanableSource);
        }

        freeCleanableSource.audioSource.clip = configAudioClip.audioClip;
        freeCleanableSource.audioSource.volume = Volume * configAudioClip.volume;

        if (configAudioClip.playWithDelay)
        {
            StartCoroutine(PlayWithDelay(freeCleanableSource.audioSource, configAudioClip.delay));
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

    public void Stop()
    {
        foreach (CleanableAudioSource cleanableAudioSource in cleanableAudioSources)
        {
            cleanableAudioSource.audioSource.Stop();
            cleanableAudioSource.lastStoppedTime = Time.time;
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
            LogNoAudioClipError(audioClipAlias, state: "playing");
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
            LogNoAudioClipError(audioClipAlias, state: "paused");
        }
    }

    private CleanableAudioSource GetPlayingAudioSource(string audioClipAlias)
    {
        return cleanableAudioSources.FirstOrDefault(source =>
            source.audioSource.isPlaying &&
            (source.audioSource.clip.name == audioClipAlias ||
            audioClips.FirstOrDefault(clip => clip.alias == audioClipAlias && clip.audioClip == source.audioSource.clip) != null)
        );
    }

    private CleanableAudioSource GetPausedAudioSource(string audioClipAlias)
    {
        return cleanableAudioSources.FirstOrDefault(source =>
            !source.audioSource.isPlaying &&
            source.audioSource.clip != null &&
            (source.audioSource.clip.name == audioClipAlias ||
            audioClips.FirstOrDefault(clip => clip.alias == audioClipAlias && clip.audioClip == source.audioSource.clip) != null)
        );
    }

    public void ChangePlayingAudioClipVolume(string audioClipAlias, float newVolume)
    {
        newVolume = Mathf.Clamp01(newVolume);

        var playingSource = GetPlayingAudioSource(audioClipAlias);

        if (playingSource != null)
        {
            playingSource.audioSource.volume = newVolume;
        }
        else
        {
            Debug.LogError("No AudioClip found playing with the alias or clip name: " + audioClipAlias);
        }
    }

    private void LogNoAudioClipError(string audioClipAlias, string state)
    {
        Debug.LogError(string.Format(NO_AUDIO_CLIP_ERROR_TEMPLATE, state) + audioClipAlias);
    }

    private void LogMultipleAudioClipsError(string audioClipAlias)
    {
        Debug.LogError(string.Format(MULTIPLE_AUDIO_CLIPS_ERROR_TEMPLATE) + audioClipAlias);
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

