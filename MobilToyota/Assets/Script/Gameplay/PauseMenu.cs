using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    
    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("PlatformMiniGame");
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
