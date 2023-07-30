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
2. Import the assets into your Unity project.
3. Add the `ServiceLocator` prefab to your scene and configure the `soundEffectPlayer` and `backgroundMusicPlayer` child objects to your liking.

## Testing

For testing purposes, a `TestSounds` script is provided. This script plays a sound when a certain key is pressed. A prefab is provided.
Use keys 1 to 4 for sound effects and 5 to 7 for background music.

## Usage

Here's an example of how to play a sound:

```csharp
ServiceLocator.Instance.Get<AudioPlayer>().Play("myAudioClipAlias");