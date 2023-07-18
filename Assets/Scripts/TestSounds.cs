using UnityAudioPlayer.Audio;
using UnityAudioPlayer.Services;
using UnityEngine;

namespace UnityAudioPlayer
{
    public class TestSounds : MonoBehaviour
    {
        private AudioPlayer soundEffectPlayer;
        private AudioPlayer backgroundMusicPlayer;

        private string[] soundEffectAliases = new string[]
        {
            "Bubbles",
            "Damaged",
            "Healed",
            "Hellstorm",
        };

        private string[] BackgroundSongsAliases = new string[]
        {
            "Cave",
            "Dungeon",
            "Sea",
        };

        private void Start()
        {
            soundEffectPlayer = ServiceLocator.Instance.SoundEffectPlayer;
            backgroundMusicPlayer = ServiceLocator.Instance.BackgroundMusicPlayer;
        }

        private void Update()
        {
            for (int i = 1; i < 5; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    soundEffectPlayer.Play(soundEffectAliases[i - 1]);
                }
            }

            for (int i = 5; i < 8; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    backgroundMusicPlayer.StopAll();
                    backgroundMusicPlayer.Play(BackgroundSongsAliases[i - 5]);
                }
            }
        }
    }
}
