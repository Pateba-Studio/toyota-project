using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

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
    public List<VideoDetail> videoDetails;
    public uiManager uiManager;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public GameObject startButton;
    public GameObject doneButton;

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        doneButton.SetActive(true);

        videoDetails.RemoveAt(0);
    } 

    public void PlayVideo(GameObject button)
    {
        button.SetActive(false);

        if (videoDetails.Count != 0)
        {
            videoPlayer.gameObject.SetActive(true);
            audioSource.gameObject.SetActive(true);
            StartCoroutine(GetAudioClip());
        }
        else
        {
            uiManager.Play();
            transform.parent.gameObject.SetActive(false);
        }
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(videoDetails[0].audioURL, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
                Debug.Log(www.error);
            else
            {
                videoPlayer.url = videoDetails[0].videoURL;
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
