using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

#if UNITY_WEBGL && !UNITY_EDITOR
using AOT;
using System.Runtime.InteropServices;
#endif

public class ContentVideo : MonoBehaviour
{

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport( "__Internal" )]
    private static extern bool CheckForWebGLIOS();
#else
    private static bool CheckForWebGLIOS()
    {
        return false;
    }
#endif

    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public RenderTexture videoTexture;

    public void CreatevideoPlayer(string mediaURL)
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoTexture;

#if !UNITY_EDITOR && UNITY_WEBGL
        if (CheckForWebGLIOS() == true)
        {
            // Webgl on mobile IOS devices will not auto play because safari wants play to be called with user intent so from an HTML button 
            videoPlayer.waitForFirstFrame = false;
            videoPlayer.playOnAwake = false;
        }
            
        // WEBGL Requires you to use Direct audio type
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
#else
        audioSource = videoPlayer.gameObject.AddComponent<AudioSource>();

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
#endif

        SetVolume(1);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.isLooping = false;

        SetVideoUrl(mediaURL);
    }

    public void SetVideoUrl(string url)
    {
        videoPlayer.url = url;

#if !UNITY_EDITOR && UNITY_WEBGL

        // If we are in a mobile IOS webgl build 
        if (CheckForWebGLIOS() == true)
        {
            // We have to use unityInstance.SendMessage to call play from an html button which requieres the name of the gameobject and the method. The method being Play
            // We use a list of video player names so we can call play on multiple players if we want 
            // Add all the names you want then we show the html button 
            IOSWebPlaybackHelper.Instance.AddToVideoList(gameObject.name);

            // The html button should be a grey overlay when you press it the it calls play with unityInstance.SendMessage satisfying the requirment of user intent 
            IOSWebPlaybackHelper.Instance.ShowHtmlButton();
        }
#endif
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    public void ToggleAudio()
    {
        if (IsMuted() == true)
        {
            UnMute();
        }
        else
        {
            Mute();
        }
    }

    public bool IsMuted()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
             
        // In webgl builds is muted can be determined with GetDirectAudioMute
        return videoPlayer.GetDirectAudioMute(0);
#else
        return videoPlayer.GetTargetAudioSource(0).mute;
#endif
    }

    public void Mute()
    {

#if !UNITY_EDITOR && UNITY_WEBGL

        // In webgl builds mute has to be set with direct audio mute
        videoPlayer.SetDirectAudioMute(0, true);
#else
        videoPlayer.GetTargetAudioSource(0).mute = true;
#endif

    }

    public void UnMute()
    {

#if !UNITY_EDITOR && UNITY_WEBGL

        // In webgl builds unmute has to be set with direct audio mute
        videoPlayer.SetDirectAudioMute(0, false);
#else
        videoPlayer.GetTargetAudioSource(0).mute = false;
#endif

    }

    public void SetVolume(float vol)
    {
        if (vol > 1)
        {
            vol = 1;
        }
        else if (vol < 0)
        {
            vol = 0;
        }

#if !UNITY_EDITOR && UNITY_WEBGL

        // In webgl build video audio will have to be managed through the direct audio 
        videoPlayer.SetDirectAudioVolume(0, vol);
#else
        audioSource.volume = vol;
#endif

    }
}