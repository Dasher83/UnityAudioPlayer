# AudioPlayer for Unity

This project provides a simple yet extensible audio player system for Unity games. It allows for flexible audio clip configuration, supports sound pooling, and is easy to integrate with any Unity project.

## Features
- **Audio Clip Registry**: Manages a collection of audio clips that can be called by alias.
- **Audio Source Pooling**: Manages a pool of audio sources for efficient audio playback.
- **Flexible AudioClip Configuration**: Each audio clip can be configured with individual volume and play settings.
- **Service Locator**: Allows for easy access to the audio player from anywhere in the code.
- **Logging**: Provides robust error logging for troubleshooting.

## Setup

To use this system, follow these steps:

1. Clone or download this repository.
2. Import the scripts into your Unity project.
3. Create an `AudioClipRegistry` and `AudioSourcePool` in your game. Use the Unity editor to add your sound files to the AudioClipRegistry and set up the AudioSourcePool.
4. Use the `ServiceLocator` to access the `AudioPlayer` from your scripts.

## Testing

For testing purposes, a `TestSounds` script is provided. This script plays a sound when a certain key is pressed.

1. Attach the `TestSounds` script to a game object.
2. In the Unity editor, assign the keys and corresponding audio clip aliases to the script.
3. During gameplay, press the assigned keys to hear the associated sounds.

## Usage

Here's an example of how to play a sound:

```csharp
ServiceLocator.Instance.Get<AudioPlayer>().Play("myAudioClipAlias");