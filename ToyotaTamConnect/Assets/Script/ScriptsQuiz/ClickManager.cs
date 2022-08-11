using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickManager : MonoBehaviour
{

    public GameObject SoalPanel, PauseButton, InstruksiPanel, PausePanel;
    public static bool cdStart;
    void Start()
    {
        SoalPanel.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("SOALE METU");
        StartCoroutine("MunculkanSoal");

    }

    private IEnumerator MunculkanSoal()
    {
        yield return new WaitForSeconds(0.3f);
        SoalPanel.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void Instruction()
    {
        StartCoroutine("TutupInstruksi");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        StartCoroutine("TutupPause");
    }

    private IEnumerator TutupPause()
    {
        yield return new WaitForSeconds(0.3f);
        PausePanel.SetActive(false);
    }
    private IEnumerator TutupInstruksi()
    {
        cdStart = true;
        yield return new WaitForSeconds(0.3f);
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine("TutupPause");
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}

