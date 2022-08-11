using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    public GameObject jawaBarat;
    public GameObject jawaTengah;
    public GameObject jawaTimur;

    public void PoskoJabar()
    {
        LeanTween.scale(jawaBarat.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }

    public void YoJabar()
    {
        //SceneManager_MiniGame_Dicky
        SceneManager.LoadScene("MainGame");
    }

    public void GakJabar()
    {
        LeanTween.scale(jawaBarat.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }

    public void PoskoJateng()
    {
        LeanTween.scale(jawaTengah.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }
    
    public void YoJateng()
    {
        SceneManager.LoadScene("montormobil");
    }

    public void GakJateng()
    {
        LeanTween.scale(jawaTengah.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }

    public void PoskoJatim()
    {
        LeanTween.scale(jawaTimur.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }
    
    public void YoJatim()
    {
        //SceneManager_MiniGame_Arlo
        SceneManager.LoadScene("QuizMinigame");
    }

    public void GakJatim()
    {
        LeanTween.scale(jawaTimur.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }
}
