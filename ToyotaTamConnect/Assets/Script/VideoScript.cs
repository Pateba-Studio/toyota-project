using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] string videoClipName;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject doneButton;
    // Start is called before the first frame update
    private void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoClipName);
        StartCoroutine(TryPlayVideo());
    }
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        doneButton.SetActive(true);
    }
    private IEnumerator TryPlayVideo()
    {
        Debug.Log("Trying to play video");
        while (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
            yield return new WaitForEndOfFrame();
        }
    }
}
