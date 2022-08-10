using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject batasAtas;
    public GameObject batasBawah;
    
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveY(batasAtas, 11, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah, -11, 1).setEaseInOutCubic();
    }

    public void ToMenu()
    {
        LeanTween.moveY(batasAtas, 3.8f, 1);
        LeanTween.moveY(batasBawah, -3.9f, 1);
        StartCoroutine(WaitAnim());
    }

    public void Exit()
    {
        Application.Quit(0);
    }

    public void MainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Menu");
    }
}
