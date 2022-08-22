using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Transform camera, forward;
    public TextMeshProUGUI nameText, originText, howText, descText;
    public GameObject infoButton, panelData;
    public Image image;
    public AudioSource audio;
    [SerializeField]
    private int imageIndex;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    
}

