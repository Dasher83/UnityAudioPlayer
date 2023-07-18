using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    [SerializeField]
    private AudioPlayer soundEffectPlayer;

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

    public AudioPlayer GetSoundEffectPlayer()
    {
        return soundEffectPlayer;
    }
}
