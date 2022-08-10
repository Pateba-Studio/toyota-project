using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSound : MonoBehaviour
{
    private static BGSound instance = null;
    public static BGSound Instance => instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        
        Invoke("PlayAudio", 3.0f);
        
        DontDestroyOnLoad(this.gameObject);
    }

    void PlayAudio()
    {
        GetComponent<AudioSource>().Play();
    }
}
