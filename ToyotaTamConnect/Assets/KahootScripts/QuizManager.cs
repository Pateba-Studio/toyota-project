using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
   
    private int gameScore = 0;
    //private float time;
    public bool isWin, sfxOn;
    public Image image;
    public int jumlahSoal = 10;
    [SerializeField] private float time;
    [SerializeField] private int soalCounter;

    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject wrongPanel;
    [SerializeField] private GameObject gameOverPanel;

    private static List<SoalData> unansweredQuestions;
    private SoalData currentQuestion;
    [SerializeField] private TextMeshProUGUI soalText;


    [SerializeField] private float timeBetweenQuestions = 1f;


    [SerializeField] private TextMeshProUGUI[] answerText;
    public SoalData[] questions;

    void Start()
    {
        sfxOn = true;
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<SoalData>();
        }

        SetCurrentQuestion();
    }

    void Update()
    {
        if (!isWin)
        {
            time += Time.deltaTime;

        }
        if (!isWin && soalCounter == jumlahSoal)
        {
            StartCoroutine(ToGameOver());
              
        }

        if (isWin)
        {
            gameOverPanel.SetActive(true);
            if (sfxOn)
            {
                sfxOn = false;
                //FindObjectOfType<AudioManager>().Play("GameOverSFX");
            }

        }

        



    }
    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        soalText.text = currentQuestion.Soal;
        image.sprite = currentQuestion.Image;

        for (int i = 0; i < 4; i++)
        {
            answerText[i].text = currentQuestion.Jawaban[i].jawaban;
        }

    }


    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);
        yield return new WaitForSeconds(timeBetweenQuestions/2);

        StartCoroutine("TutupSoal");

    }

    public void UserSelectTrue(int i)
    {
        if (currentQuestion.Jawaban[i].isTrue)
        {
            isWin = false;
            gameScore += 10;

            StartCoroutine(CorrectAnswer());

        }
        else
        {
            isWin = false;
            foreach (Answer jawaban in currentQuestion.Jawaban)
            {
                if (jawaban.isTrue)
                {
                 
                }
            }
            StartCoroutine(WrongAnswer());
        }

        soalCounter++;
        

        if (!isWin)
        {
            if (soalCounter != jumlahSoal)
            {
                StartCoroutine(TransitionToNextQuestion());
            }
                
        }
    }

    private IEnumerator TutupSoal()
    {
        yield return new WaitForSeconds(0.3f);
        Start();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator CorrectAnswer()
    {
        correctPanel.SetActive(true);
        //FindObjectOfType<AudioManager>().Play("CorrectSFX");
        yield return new WaitForSeconds(timeBetweenQuestions);
        correctPanel.SetActive(false);
    }

    private IEnumerator WrongAnswer()
    {
        wrongPanel.SetActive(true);
        //FindObjectOfType<AudioManager>().Play("WrongSFX");
        yield return new WaitForSeconds(timeBetweenQuestions);

        wrongPanel.SetActive(false);
    }

    private IEnumerator ToGameOver()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        isWin = true;
    }


}
