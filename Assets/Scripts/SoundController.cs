using System;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    public enum SoundName
    {
        MenuBGM,
        PlayBGM,
        BossBGM,
        Death,
        GameOver,
        Upgrade,
        Buy,
        Pickup,
        Damage
    }

    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioSource BGM;
    private SoundName currentTrack;

    private void Start()
    {
        Play(SoundName.MenuBGM);
    }

    public void Play(SoundName name)
    {
        switch (name)
        {
            case SoundName.MenuBGM:
                PlayBGM(name);
                break;
            case SoundName.PlayBGM:
                PlayBGM(name);
                break;
            case SoundName.BossBGM:
                PlayBGM(name);
                break;
            case SoundName.Death:
                PlayEffect(name);
                break;
            case SoundName.GameOver:
                PlayEffect(name);
                break;
            case SoundName.Upgrade:
                PlayEffect(name);
                break;
            case SoundName.Buy:
                PlayEffect(name);
                break;
            case SoundName.Pickup:
                PlayEffect(name);
                break;
            case SoundName.Damage:
                PlayEffect(name);
                break;
            default:
                PlayEffect(name);
                break;
        }
    }

    private void PlayEffect(SoundName name)
    {
        if(name == SoundName.GameOver)
        {
            BGM.Stop();
        }
        Sound sound = GetSound(name);
        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volumn;
            sound.audioSource.loop = sound.loop;
        }
        sound.audioSource.Play();
    }

    private void PlayBGM(SoundName name)
    {
        Sound sound = GetSound(name);
        if (BGM.clip == null || currentTrack != name)
        {
            currentTrack = name;
            BGM.clip = sound.clip;
            BGM.volume = sound.volumn;
            BGM.loop = sound.loop;
            BGM.Play();

        }
    }

    private Sound GetSound(SoundName name)
    {
        return Array.Find(sounds, s => s.soundName == name);
    }
}
