using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class AudioSound
{
    public string name;

    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0f,1f)]
    public float pitch;

    public AudioMixerGroup mixerGroup;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
