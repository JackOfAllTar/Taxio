using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public SoundController.SoundName soundName;

    public AudioClip clip;
    [Range(0f, 1f)] public float volumn;
    public bool loop;
    [HideInInspector] public AudioSource audioSource;
}
