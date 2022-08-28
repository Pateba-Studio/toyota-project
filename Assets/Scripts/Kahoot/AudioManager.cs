using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.playOnAwake = s.onAwake;
            //s.source.PlayOneShot(s.clip) = s.oneShot;
        }
    }

    void Start()
    {
        //Play("Theme");
    }

    public void ButtonClick()
    {
        //PlayOneShot("Click");
    }

    public void Play(string name)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;

        }
           
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;

        }

        s.source.PlayOneShot(s.source.clip);
    }

    public void Mute(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;

        }

        s.source.volume = 0;
    }

    public void UnMute(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }

        if (s.name == "MuseumTheme" || s.name == "ButtonSFX" || s.name == "Button2SFX" || s.name == "InstSFX")
        {
            s.source.volume = 0.1f;
        }
        else
        {
            s.source.volume = 0.1f;
        }
    }
    //FindObjectOfType<AudioManager>().Play("Show");
}
