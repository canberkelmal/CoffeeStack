using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    Sound s;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        s.source.time = s.startTime;
        s.source.Play();
/*
        if (!s.source.isPlaying)
        {
            s.source.time = s.startTime;
            s.source.Play();
            Invoke("StopSound", s.endTime);
        }
*/
    }

    void StopSound(Sound a) 
    {
        s.source.Stop();
    }
}
