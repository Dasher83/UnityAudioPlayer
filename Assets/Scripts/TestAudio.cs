using UnityAudioPlayer.Audio;
using UnityAudioPlayer.Services;

using UnityEngine.InputSystem;
using UnityEngine;

namespace UnityAudioPlayer
{
    /*
     * Press 1-4 keys to test sound effects
     * Press 5-8 keys to test songs / background music
     */
    public class TestAudio : MonoBehaviour
    {
        [SerializeField] private InputActionAsset testAudioActions;

        private InputActionMap soundEffectsActionMap;
        private InputActionMap backgroundMusicActionMap;
        private AudioPlayer soundEffectPlayer;
        private AudioPlayer backgroundMusicPlayer;

        private readonly string[] soundEffectAliases = new string[]
        {
            "Bubbles",
            "Damaged",
            "Healed",
            "Hellstorm",
        };

        private readonly string[] backgroundSongsAliases = new string[]
        {
            "Cave",
            "Dungeon",
            "Sea",
        };

        private void Awake()
        {
            soundEffectPlayer = ServiceLocator.Instance.SoundEffectPlayer;
            backgroundMusicPlayer = ServiceLocator.Instance.BackgroundMusicPlayer;

            soundEffectsActionMap = testAudioActions.FindActionMap("SoundEffects", true);
            backgroundMusicActionMap = testAudioActions.FindActionMap("BackgroundMusic", true);

            for (int i = 1; i <= soundEffectAliases.Length; i++)
            {
                int lambdaIndex = i - 1;
                soundEffectsActionMap.FindAction("SoundEffect_" + i, true).performed +=
                    _ => soundEffectPlayer.Play(soundEffectAliases[lambdaIndex]);
            }

            for (int i = 1; i <= backgroundSongsAliases.Length; i++)
            {
                int lambdaIndex = i - 1;
                backgroundMusicActionMap.FindAction("BackgroundMusic_" + i, true).performed +=
                    _ =>
                    {
                        backgroundMusicPlayer.StopAll();
                        backgroundMusicPlayer.Play(backgroundSongsAliases[lambdaIndex]);
                    };
            }
        }

        private void OnEnable()
        {
            soundEffectsActionMap.Enable();
            backgroundMusicActionMap.Enable();
        }

        private void OnDisable()
        {
            soundEffectsActionMap.Disable();
            backgroundMusicActionMap.Disable();
        }
    }
}
