using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            return;
        }

        foreach(AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("lobbyMusic");
    }

    public void Play(string name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        s.source.Play();
    }

    //inumerant here to yield volume down and up?
    public void goingCombat()
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == "lobbyMusic");
        if(s == null){return;}
        s.source.Stop();
        // s.source.volume = 0;
        Play("combatMusic");
    }
    public void goingOffCombat()
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == "combatMusic");
        if(s == null){return;}
        // s.source.volume = 0;
        s.source.Stop();

        AudioSound l = Array.Find(sounds, sound => sound.name == "lobbyMusic");
        if(l == null){return;}
        // l.source.volume = 0.07f;
        l.source.Play();

    }
}
