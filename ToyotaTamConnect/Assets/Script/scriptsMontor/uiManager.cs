using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class uiManager : MonoBehaviour
{
    [SerializeField] GameObject menu, intro, gameOver, winCardPanel, tingkatkanSkor, medalObj;
    [SerializeField] Sprite[] cardSpriteList, medalSpriteList;
    [SerializeField] Image winCard;
    Animator animator;
    public TextMeshProUGUI scoreText, achText;
    int score, firstRun = 0;
    private bool isGetCard;
    public AudioManager am;

    // Use this for initialization
    void Start()
    {
        Pause();
        animator = scoreText.GetComponent<Animator>();
        //PlayerPrefs.SetInt("HasPlayed", 0); //reset tutor nyalakan ini lol
        //firstRun = PlayerPrefs.GetInt("HasPlayed");
        //if (firstRun == 0) // remember "==" for comparing, not "=" which assigns value
        //{
        //    PlayerPrefs.SetInt("HasPlayed", 1);
        //    Pause();
        //    intro.SetActive(true);
        //}
        //else
        //{
        //    //Do lots of game save loading
        //}
        InvokeRepeating("scoreUpdate", 1.0f, 1.0f);
        am.carSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }

    void scoreUpdate()
    {
        animator.SetTrigger("Change");
        score += 1;
    }
    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            if (am != null)
            {
                am.carSound.Stop();
            }

        }
    }

    public void Play()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            if(am != null)
            {
                am.carSound.Play();
            }
        }
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void GameOverActivated()
    {
        Pause();
        gameOver.SetActive(true);
        //if (score > 60)
        //{
        //    SetMedal(0); //medal number, 0 = emas, 1 = perak, 2 = perunggu
        //    switch (Random.Range(0, 2)) //100% dapet card random jateng
        //    {
        //        case 0:
        //            DapetKartuJateng(1);
        //            break;
        //        case 1:
        //            DapetKartuJateng(2);
        //            break;
        //    }
        //}
        //else if (score > 40)
        //{
        //    SetMedal(1);
        //    if (Random.Range(0, 3) >= 1) //66,6% dapet card jateng?
        //    {
        //        switch (Random.Range(0, 2))
        //        {
        //            case 0:
        //                DapetKartuJateng(1);
        //                break;
        //            case 1:
        //                DapetKartuJateng(2);
        //                break;
        //        }
        //    }
        //}
        //else if (score > 20)
        //{
        //    SetMedal(2);
        //    if (Random.Range(0, 3) == 2) //33,3% dapet card jateng?
        //    {
        //        switch (Random.Range(0, 2))
        //        {
        //            case 0:
        //                DapetKartuJateng(1);
        //                break;
        //            case 1:
        //                DapetKartuJateng(2);
        //                break;
        //        }
        //    }
        //}
        //else
        //{
        //    tingkatkanSkor.SetActive(true);
        //}
        //achText.text = scoreText.GetParsedText();
    }
    private void DapetKartuJateng(int number)
    {
        winCard.sprite = cardSpriteList[number - 1];
        winCardPanel.SetActive(true);
        PlayerPrefs.SetInt("Jateng"+number, 1);
        Debug.Log("dpt Jateng " + number);
    }
    void SetMedal(int medalNum)
    {
        medalObj.GetComponent<Image>().sprite = medalSpriteList[medalNum];
        medalObj.SetActive(true);
    }
    public void WinActivated()
    {
        Pause();
        winCardPanel.SetActive(true);
    }
    public void OpenMenu()
    {
        menu.gameObject.SetActive(true);
    }
    public void CloseMenu()
    {
        menu.gameObject.SetActive(false);
    }
    public void ResetTutor()
    {
        PlayerPrefs.SetInt("HasPlayed", 0);
    }
    public void OKCardButton()
    {
        if (isGetCard == false)
        {
            Debug.Log("tidak dapet kartu");
            Replay();
        }

        if (isGetCard == true)
        {
            Debug.Log("dapet kartu");
            winCardPanel.SetActive(true);
            //popCardPanel.GetComponent<Tweener>().PopCard();
        }
    }
}