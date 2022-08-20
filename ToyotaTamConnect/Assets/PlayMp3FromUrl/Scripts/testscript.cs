using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testscript : MonoBehaviour
{
	
    AudioSource audioSource;
    AudioClip myClip;
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
        StartCoroutine(GetAudioClip());
		Debug.Log("Starting to download the audio...");
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://ciihuy.com/downloads/music.mp3", AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.Play();
				Debug.Log("Audio is playing.");
            }
        }
    }
	
	
	public void pauseAudio(){
		audioSource.Pause();
	}
	
	public void playAudio(){
		audioSource.Play();
	}
	
	public void stopAudio(){
		audioSource.Stop();
	
	}
}