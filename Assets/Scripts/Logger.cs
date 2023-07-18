using UnityEngine;

public class Logger {

    private const string NO_ALIAS_OR_NAME_PROVIDED_MESSAGE = "No alias or name provided. Cannot play AudioClip.";

    private const string NO_AUDIO_CLIP_ERROR_TEMPLATE = "No AudioClip found {0} with the alias or clip name: ";
    private const string MULTIPLE_AUDIO_CLIPS_ERROR_TEMPLATE = "Multiple ConfigurableAudioClips found with the same alias: ";

    private void Log(string message)
    {
        Debug.Log(message);
    }

    public void LogNoAliasOrNameProvided()
    {
        this.Log(NO_ALIAS_OR_NAME_PROVIDED_MESSAGE);
    }

    public void LogNoAudioClipError(string audioClipAlias, string state = "")
    {
        this.Log(message: string.Format(NO_AUDIO_CLIP_ERROR_TEMPLATE, state) + audioClipAlias);
    }

    public void LogMultipleAudioClipsError(string audioClipAlias)
    {
        this.Log(message: string.Format(MULTIPLE_AUDIO_CLIPS_ERROR_TEMPLATE) + audioClipAlias);
    }
}
