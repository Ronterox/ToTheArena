﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private float timer = 0;
    void Awake()
    {
        MakeSingleton();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            Debug.LogWarning("There's no Sound Effect with the name " + name);
        else
            s.source.Play();
    }
    
    public void Play(string name, float delay)
    {
        if (timer > 0)
            return;

        timer = delay;

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            Debug.LogWarning("There's no Sound Effect with the name " + name);
        else
            s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            Debug.LogWarning("There's no Sound Effect with the name " + name);
        else
            s.source.Stop();
    }
}
