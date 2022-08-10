using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerPlatform : MonoBehaviour
{
    public Question[] questions;
    public GameObject soalPanel, PauseButton, popUpTrue, popUpFalse, popUpTimeUp, popWin, popLose, popfive, losefive;
    private int gameScore = 0;
    private float time;
    public string[] kotaNama;
    public bool isWin;

    private static List<Question> unansweredQuestions;

    private Question currentQuestion;

    [SerializeField] private TextMeshProUGUI soalText;

    [SerializeField] private TextMeshProUGUI platText;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private TextMeshProUGUI kotaKanan;

    [SerializeField] private TextMeshProUGUI kotaKiri;

    [SerializeField] private float timeBetweenQuestions = 1f;

    [SerializeField] private TextMeshProUGUI[] answerText;

    void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        SetCurrentQuestion();
        SetRandomKota();
    }

    void Update()
    {
      
        if (ClickManager.cdStart)
        {
            ClickManager.cdStart = false;
            StartCoroutine(StartCountdown(300)); // WAKTU MENJAWAB
        }

        if (isWin) 
        {
            StartCoroutine(GameOver(1f));
        }

    }
    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        soalText.text = currentQuestion.soal;
        platText.text = currentQuestion.plat;

        for (int i=0; i < 4;i++)
        {
            answerText[i].text = currentQuestion.answers[i].jawaban;
        }
    }
    public IEnumerator StartCountdown(float countdownValue)
    {
        time = countdownValue;
        while (time > 0)
        {
            Debug.Log("Countdown: " + time);
            yield return new WaitForSeconds(1.0f);
            time--;
            
            if(time <= 0 && !isWin)
            {
                time = 0;
                ClickManager.cdStart = false;
                Debug.Log("WAKTU HABIS");
                soalPanel.GetComponent<Tweener>().OnClose();

                popUpTimeUp.SetActive(true);
                popUpTimeUp.GetComponent<Tweener>().TimeUp();

                StartCoroutine(GameOver(5f)); // POP UP KALAH (RESTART HOME)
            
            }
            timerText.text = time.ToString();
        }   
    }

    IEnumerator GameOver(float time)
    {
        
        yield return new WaitForSeconds(time);
        PauseButton.GetComponent<Button>().onClick.Invoke();

    }
    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);
        yield return new WaitForSeconds(timeBetweenQuestions);
        
        StartCoroutine("TutupSoal");
    }

    public void UserSelectTrue(int i)
    {
        if (currentQuestion.answers[i].isTrue)
        {
            Debug.Log("CORRECT");
            gameScore += 5;

            SceneManager.LoadScene("PlatformMinigame");
            
            scoreText.text = "" + gameScore.ToString();
            popUpTrue.SetActive(true);
            popUpTrue.GetComponent<Tweener>().PopUp();
            
            popfive.SetActive(true);
            popfive.GetComponent<Tweener>().PopUp();

            kotaKanan.GetComponent<Tweener>().OnClose();
            kotaKiri.GetComponent<Tweener>().OnClose();

            
            
            if (gameScore == 100 && !isWin) //BATAS SKOR
            {
                isWin = true;
                Debug.Log("GANTI LEVEL"); // POP UP MENANG BACK TO MAIN MENU WITH UNLOCK NEW MAP

                popUpTrue.SetActive(true);
                popUpTrue.GetComponent<Tweener>().PopUp();

                popfive.SetActive(true);
                popfive.GetComponent<Tweener>().PopUp();

                popWin.SetActive(true);
                popWin.GetComponent<Tweener>().WinUp();

            }
        }
        else
        {
            SceneManager.LoadScene("PlatformMinigame");
            isWin = false;
            Debug.Log("WRONG");
            time -= 5;

            popUpFalse.SetActive(true);
            popUpFalse.GetComponent<Tweener>().PopUp();

            losefive.SetActive(true);
            losefive.GetComponent<Tweener>().PopUp();

            kotaKanan.GetComponent<Tweener>().OnClose();
            kotaKiri.GetComponent<Tweener>().OnClose();
        }
        
        StartCoroutine(TransitionToNextQuestion());
    }

    private IEnumerator TutupSoal()
    {
        yield return new WaitForSeconds(0.3f);
        PauseButton.SetActive(true);
        kotaKanan.gameObject.SetActive(true);
        kotaKiri.gameObject.SetActive(true);
        Start();
    }

    public void ButtonClickA()
    {
        UserSelectTrue(0);
    }
    public void ButtonClickB()
    {
        UserSelectTrue(1);
    }
    public void ButtonClickC()
    {
        UserSelectTrue(2);
    }
    public void ButtonClickD()
    {
        UserSelectTrue(3);
    }

    public void ButtonClickX()
    {
        StartCoroutine("TutupSoal");
        
        SceneManager.LoadScene("PlatformMinigame");
    }

    void SetRandomKota()
    {
        int i = Random.Range(0, 27);
        int j = Random.Range(0, 27);
        if(i == j)
        {
            SetRandomKota();
        }
        kotaKanan.text = kotaNama[i].ToString();
        kotaKiri.text = kotaNama[j].ToString();
    }
}
 