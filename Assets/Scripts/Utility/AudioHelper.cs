using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    private static List<AudioSource> currentLoopingClips = new List<AudioSource>();

    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audio = new GameObject("Audio2D");
        AudioSource audioSource = audio.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();
        Object.Destroy(audio, clip.length);

        return audioSource;
    }

    public static AudioSource PlayClipLoop(AudioClip clip, float volume)
    {
        GameObject audio = new GameObject("AudioLooping2D");
        AudioSource audioSource = audio.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;

        audioSource.Play();

        return audioSource;
    }
}
