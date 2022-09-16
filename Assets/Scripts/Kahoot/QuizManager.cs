using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public bool isWin, sfxOn;
    public GameManager gameManager;

    [Header("Question Display Attribute")]
    public int jumlahSoal;
    public int soalCounter;
    public float timeBetweenQuestions;
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject logoTAM;
    public Text soalText;
    public Text[] answerText;
    public Button[] answerButton;
    public SoalData[] questions;
    public UnityEvent videoIsFinished;

    [Header("Panel Attribute")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject[] gameOver;

    List<SoalData> unansweredQuestions;
    SoalData currentQuestion;

    void Awake()
    {
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        GetQuestion();
    }

    void Start()
    {
        sfxOn = true;
    }

    void Update()
    {
        if (!isWin && soalCounter == jumlahSoal)
        {
            StartCoroutine(ToGameOver());
        }

        if (isWin)
        {
            if (sfxOn)
            {
                sfxOn = false;
                FindObjectOfType<AudioManager>().Play("GameOverSFX");
            }

            bool isDone = false;
            for (int j = 0; j < gameManager.questionInfos.Count; j++)
            {
                if (gameManager.questionInfos[j].questionDetails.Count > 0)
                {
                    isDone = false;
                    break;
                }
                else
                    isDone = true;
            }

            if (gameManager.getQuestion.hallType == HallType.PDP)
            {
                if (!isDone)
                {
                    gameOver[0].SetActive(true);
                    StartCoroutine(gameManager.StartGame(2.5f));
                }
                else
                {
                    gameOver[int.Parse(gameManager.subMasterValueId) - 1].SetActive(true);
                    StartCoroutine(gameManager.getQuestion.PostLastCheckpoint());
                }
            }
            else
            {
                if (isDone)
                {
                    gameOver[1].SetActive(true);
                    StartCoroutine(gameManager.getQuestion.PostLastCheckpoint());
                    //StartCoroutine(gameManager.OpenRoom(gameManager.getQuestion.hallABURL));
                }
                else
                {
                    gameOver[0].SetActive(true);
                    StartCoroutine(gameManager.StartGame(2.5f));
                }
            }
        }
    }

    void EnableAnswer()
    {
        for (int i = 0; i < 4; i++)
        {
            answerButton[i].interactable = true;
        }
    }

    void DisableAnswer(int i)
    {
        answerButton[i].interactable = false;
    }

    void StartQuestion()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<SoalData>();
        }
        SetCurrentQuestion();
    }

    public void SetCurrentQuestion()
    {
        logoTAM.SetActive(true);
        imageHandler.SetActive(false);
        videoHandler.SetActive(false);

        currentQuestion = unansweredQuestions[0];
        soalText.text = currentQuestion.Soal;

        if (currentQuestion.mediaURL != string.Empty &&
            currentQuestion.audioURL != string.Empty)
        {
            logoTAM.SetActive(false);
            videoHandler.SetActive(true);

            for (int i = 0; i < answerButton.Length; i++)
                answerButton[i].interactable = false;

            videoHandler.GetComponent<VideoScript>().videoDetails.videoURL = currentQuestion.mediaURL;
            videoHandler.GetComponent<VideoScript>().videoDetails.audioURL = currentQuestion.audioURL;
            videoHandler.GetComponent<VideoScript>().PlayVideo(videoIsFinished);
        }
        else if (currentQuestion.mediaURL != string.Empty)
        {
            logoTAM.SetActive(false);
            imageHandler.SetActive(true);

            StartCoroutine(ProcessImageAttribute(currentQuestion.mediaURL));
            EnableAnswer();
        }
        else
        {
            logoTAM.SetActive(true);
            EnableAnswer();
        }

        for (int i = 0; i < 4; i++)
        {
            answerText[i].text = currentQuestion.Jawaban[i].jawaban;
        }
    }

    public void GetQuestion()
    {
        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                questions = new SoalData[gameManager.questionInfos[i].questionDetails.Count];
                jumlahSoal = questions.Length;

                for (int j = 0; j < gameManager.questionInfos[i].questionDetails.Count; j++)
                {
                    string mediaURL = gameManager.questionInfos[i].questionDetails[j].media;
                    string audioURL = gameManager.questionInfos[i].questionDetails[j].audio;
                    string jsonSoal = gameManager.questionInfos[i].questionDetails[j].question;
                    List<string> jsonAnswer = new List<string>();
                    string jsonCorrectAnswer = string.Empty;

                    for (int k = 0; k < gameManager.questionInfos[i].questionDetails[j].answerDetails.Length; k++)
                    {
                        jsonAnswer.Add(gameManager.questionInfos[i].questionDetails[j].answerDetails[k].answer);
                        if (gameManager.questionInfos[i].questionDetails[j].answerDetails[k].isCorrect)
                            jsonCorrectAnswer = gameManager.questionInfos[i].questionDetails[j].answerDetails[k].answer;
                    }

                    SetQuestion(j, mediaURL, audioURL, jsonSoal, jsonAnswer, jsonCorrectAnswer);
                }

                gameManager.questionInfos[i].questionDetails.Clear();
            }
        }

        StartQuestion();
    }

    public void SetQuestion(int counter, string _mediaURL, string _audioURL, string _soal, List<string> _answer, string _correctAnswer)
    {
        SoalData s = ScriptableObject.CreateInstance<SoalData>();
        s.SetMediaURL(_mediaURL);
        s.SetAudioURL(_audioURL);
        s.SetSoal(_soal);

        for (int i = 0; i < _answer.Count; i++)
            s.SetAnswer(counter, _answer[i]);

        s.SetCorrectAnswer(_correctAnswer);
        questions[counter] = s;
    }

    public void UserSelectTrue(int i)
    {
        if (videoHandler.GetComponent<VideoScript>().videoPlayer.renderMode != VideoRenderMode.CameraNearPlane)
        {
            if (currentQuestion.Jawaban[i].isTrue)
            {
                isWin = false;
                soalCounter++;

                StartCoroutine(CorrectAnswer());

                if (soalCounter < jumlahSoal)
                    StartCoroutine(TransitionToNextQuestion());

                logoTAM.SetActive(true);
                videoHandler.SetActive(false);
                imageHandler.SetActive(false);
            }
            else
            {
                isWin = false;
                DisableAnswer(i);
                StartCoroutine(WrongAnswer());
            }
        }
    }

    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);
        yield return new WaitForSeconds(timeBetweenQuestions/2);

        StartCoroutine(TutupSoal());
    }

    IEnumerator TutupSoal()
    {
        yield return new WaitForSeconds(0.3f);
        StartQuestion();
    }

    IEnumerator CorrectAnswer()
    {
        correctPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("CorrectSFX");
        yield return new WaitForSeconds(3f);
        correctPanel.SetActive(false);
    }

    IEnumerator WrongAnswer()
    {
        wrongPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("WrongSFX");
        yield return new WaitForSeconds(3f);
        wrongPanel.SetActive(false);
    }

    IEnumerator ToGameOver()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        isWin = true;
        StartCoroutine(gameManager.StartGame(2.5f));
    }

    IEnumerator ProcessImageAttribute(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler.GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
        imageHandler.GetComponent<Image>().preserveAspect = true;
    }
}