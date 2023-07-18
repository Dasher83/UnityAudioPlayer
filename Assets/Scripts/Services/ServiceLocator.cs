using UnityAudioPlayer.Audio;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    [SerializeField]
    private AudioPlayer soundEffectPlayer;

    [SerializeField]
    private AudioPlayer backgroundMusicPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioPlayer SoundEffectPlayer => soundEffectPlayer;

    public AudioPlayer BackgroundMusicPlayer => backgroundMusicPlayer;
}
