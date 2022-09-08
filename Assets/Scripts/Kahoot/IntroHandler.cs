using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[System.Serializable]
public class VideoIntro
{
    [TextArea(2, 5)]
    public string videoURL;
    [TextArea(2, 5)]
    public string audioURL;
}

public class IntroHandler : MonoBehaviour
{
    public GameObject gameManager;

    [Header("Video Attribute")]
    public bool isVideo;
    public List<VideoIntro> videoDetails;
    public GameObject videoPlayer;
    public TextMeshProUGUI instructionText;
    public GameObject playButton;

    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(() => PlayVideo());
        videoPlayer.GetComponent<VideoPlayer>().loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        instructionText.gameObject.SetActive(true);
        videoPlayer.gameObject.SetActive(false);
        playButton.SetActive(true);

        videoDetails.RemoveAt(0);
        if (videoDetails.Count == 0)
            instructionText.text = "Selamat Bermain!";
    } 

    public void PlayVideo()
    {
        playButton.SetActive(false);

        if (videoDetails.Count != 0)
        {
            instructionText.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(true);
            StartCoroutine(GetAudioClip());
        }
        else
        {
            gameManager.SetActive(true);
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
                videoPlayer.GetComponent<VideoPlayer>().url = videoDetails[0].videoURL;
                videoPlayer.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);

                videoPlayer.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;
                videoPlayer.GetComponent<VideoPlayer>().EnableAudioTrack(0, false);
                videoPlayer.GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);

                videoPlayer.GetComponent<VideoPlayer>().Play();
                videoPlayer.GetComponent<AudioSource>().Play();
            }
        }
    }
}
