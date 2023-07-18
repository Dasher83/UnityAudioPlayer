using UnityEngine;

[System.Serializable]
public class ConfigurableAudioClip
{
    private const float DEFAULT_VOLUME = 1f;
    private const float DEFAULT_DELAY = 0f;

    public AudioClip audioClip;
    [Tooltip("A convenient name for use in the code.")]
    public string alias;
    [Tooltip("Volume level for this specific AudioClip, relative to the MasterVolume. Value should be between 0 and 1.")]
    [Range(0f, 1f)]
    public float volume = DEFAULT_VOLUME;
    [Tooltip("Play this AudioClip with delay or not.")]
    public bool playWithDelay = false;
    [Tooltip("Seconds before playing this AudioClip. Value should be greater than or equal to 0.")]
    public float delay = DEFAULT_DELAY;

    // Add more configuration options as needed
}
