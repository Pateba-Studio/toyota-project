using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    //[SerializeField] string url;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject doneButton;
    // Start is called before the first frame update
    private void Awake()
    {
        //videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, url);
    }
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        doneButton.SetActive(true);
    }
}
