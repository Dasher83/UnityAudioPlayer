using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioSourceConfig
{
    private const bool DEFAULT_PLAY_ON_AWAKE = false;
    private const bool DEFAULT_LOOP = false;
    private const AudioRolloffMode DEFAULT_ROLLOFF_MODE = AudioRolloffMode.Logarithmic;
    private const float DEFAULT_MIN_DISTANCE = 1.0f;
    private const float DEFAULT_MAX_DISTANCE = 500.0f;
    // Define more default values as needed

    public bool playOnAwake = DEFAULT_PLAY_ON_AWAKE;
    public bool loop = DEFAULT_LOOP;
    public AudioRolloffMode rolloffMode = DEFAULT_ROLLOFF_MODE;
    public float minDistance = DEFAULT_MIN_DISTANCE;
    public float maxDistance = DEFAULT_MAX_DISTANCE;
    public AudioMixerGroup outputAudioMixerGroup;
    // Add more configuration options as needed
}
