using System;
using System.Linq;
using UnityEngine;

namespace UnityAudioPlayer.Audio.Helpers
{
    [Serializable]
    public class AudioClipRegistry
    {
        [SerializeField]
        private ConfigurableAudioClip[] audioClips;

        private Logger logger = new Logger();

        private ConfigurableAudioClip GetAudioClipByAlias(string audioClipAlias)
        {
            return audioClips.FirstOrDefault(clip => clip.alias == audioClipAlias);
        }

        private ConfigurableAudioClip GetAudioClipByName(string audioClipName)
        {
            return audioClips.FirstOrDefault(clip => clip.audioClip.name == audioClipName);
        }

        public ConfigurableAudioClip GetAudioClip(string aliasOrName)
        {
            if (string.IsNullOrEmpty(aliasOrName))
            {
                logger.LogNoAliasOrNameProvided();
                return null;
            }

            ConfigurableAudioClip configAudioClip = this.GetAudioClipByAlias(aliasOrName);

            if (configAudioClip == null)
            {
                configAudioClip = GetAudioClipByName(aliasOrName);

                if (configAudioClip == null)
                {
                    logger.LogNoAudioClipError(aliasOrName);
                    return null;
                }
            }

            if (audioClips.Count(clip => clip.alias == aliasOrName) > 1)
            {
                logger.LogMultipleAudioClipsError(aliasOrName);
                return null;
            }

            return configAudioClip;
        }

        public void ChangeClipVolume(string aliasOrName, float newVolume)
        {
            ConfigurableAudioClip configurableAudioClip = GetAudioClip(aliasOrName);

            if (configurableAudioClip == null) return;

            configurableAudioClip.volume = Mathf.Clamp01(newVolume);
        }
    }
}
