using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public bool isMute;
    public Button muteButton;
    public Sprite muteSprite;
    public Sprite unmuteSprite;
    public AudioSource bgmSource;
    public List<AudioSource> audioSources;
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

        isMute = Convert.ToBoolean(PlayerPrefs.GetInt("Mute"));
    }

    void Update()
    {
        if (muteButton == null)
        {
            audioSources.Clear();

            var audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource sources in audios)
            {
                audioSources.Add(sources);
                if (isMute) sources.mute = true;
                else sources.mute = false;
            }

            muteButton = GameObject.Find("Mute Button").GetComponent<Button>();
            muteButton.onClick.AddListener(() => MuteButtonClicked());

            if (isMute) muteButton.GetComponent<Image>().sprite = unmuteSprite;
            else muteButton.GetComponent<Image>().sprite = muteSprite;
        }
    }

    public void MuteButtonClicked()
    {
        if (!isMute)
        {
            isMute = true;
            muteButton.GetComponent<Image>().sprite = unmuteSprite;
            PlayerPrefs.SetInt("Mute", 1);

            foreach (AudioSource sources in audioSources)
                sources.mute = true;
        }
        else
        {
            isMute = false;
            muteButton.GetComponent<Image>().sprite = muteSprite;
            PlayerPrefs.SetInt("Mute", 0);

            foreach (AudioSource sources in audioSources)
                sources.mute = false;
        }
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
}