using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class VideoDetail
{
    [TextArea(2, 5)]
    public string videoURL;
    [TextArea(2, 5)]
    public string audioURL;
}

public class VideoScript : MonoBehaviour
{
    public bool isFullScreen;
    public GameObject videoFullScreen;
    public VideoDetail videoDetails;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public RenderTexture targetTexture;
    public GameObject[] buttonNormalScreen;
    public GameObject[] buttonFullScreen;
    public UnityEvent videoIsFinished;

    void Update()
    {
        if (gameObject.activeInHierarchy)
            SetVideoCanvas();
    }

    void EndReached(VideoPlayer vp)
    {
        videoIsFinished.Invoke();
        audioSource.Stop();

        if (isFullScreen)
            SetFullScreen();
    }

    public void PlayVideo()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        StartCoroutine(GetAudioClip());
    }

    public void PlayVideo(UnityEvent events)
    {
        videoIsFinished = events;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        StartCoroutine(GetAudioClip());
    }

    public void SetFullScreen()
    {
        if (!isFullScreen)
        {
            if (!videoPlayer.isPlaying)
                PlayVideo();

            videoFullScreen.SetActive(true);
            isFullScreen = true;
        }
        else
        {
            videoFullScreen.SetActive(false);
            isFullScreen = false;
        }
    }

    public void SetVideoCanvas()
    {
        if (!isFullScreen)
        {
            for (int i = 0; i < buttonNormalScreen.Length; i++)
                buttonNormalScreen[i].SetActive(true);

            for (int i = 0; i < buttonFullScreen.Length; i++)
                buttonFullScreen[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < buttonNormalScreen.Length; i++)
                buttonNormalScreen[i].SetActive(false);

            for (int i = 0; i < buttonFullScreen.Length; i++)
                buttonFullScreen[i].SetActive(true);
        }
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(videoDetails.audioURL, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
                Debug.Log(www.error);
            else
            {
                videoPlayer.url = videoDetails.videoURL;
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                
                videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
                videoPlayer.EnableAudioTrack(0, false);
                videoPlayer.SetDirectAudioMute(0, true);

                videoPlayer.Play();
                audioSource.Play();
            }
        }
    }
}
