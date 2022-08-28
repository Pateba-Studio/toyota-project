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
    public VideoDetail videoDetails;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public GameObject replayButton;
    public UnityEvent videoIsFinished;

    void EndReached(VideoPlayer vp)
    {
        videoIsFinished.Invoke();
        replayButton.SetActive(true);
    }

    public void PlayVideo()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        replayButton.SetActive(false);
        StartCoroutine(GetAudioClip());
    }

    public void PlayVideo(UnityEvent events)
    {
        videoIsFinished = events;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        replayButton.SetActive(false);
        StartCoroutine(GetAudioClip());
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
