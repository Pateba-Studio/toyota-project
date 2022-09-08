using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource am;
    public GameObject correctAnswer;
    public GameObject wrongAnswer;
    public UnityEvent videoIsFinished;

    [Header("Panel Attributes")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject[] gameOver;

    [Header("Question Attributes")]
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject logoTAM;
    public Text leftAns;
    public Text rightAns;
    public Text soal;

    // Use this for initialization
    void Start()
    {
        Play();

        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        StartCoroutine(SetQuestion());
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            if (am != null)
            {
                am.Stop();
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
                am.Play();
            }
        }
    }

    public IEnumerator SetQuestion()
    {
        logoTAM.SetActive(true);
        imageHandler.SetActive(false);
        videoHandler.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        
        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                if (gameManager.questionInfos[i].questionDetails[0].media != string.Empty &&
                    gameManager.questionInfos[i].questionDetails[0].audio != string.Empty)
                {
                    logoTAM.SetActive(false);
                    videoHandler.SetActive(true);

                    videoHandler.GetComponent<VideoScript>().videoDetails.videoURL = gameManager.questionInfos[i].questionDetails[0].media;
                    videoHandler.GetComponent<VideoScript>().videoDetails.audioURL = gameManager.questionInfos[i].questionDetails[0].audio;
                    videoHandler.GetComponent<VideoScript>().PlayVideo(videoIsFinished);

                    Pause();
                }
                else if (gameManager.questionInfos[i].questionDetails[0].media != string.Empty)
                {
                    logoTAM.SetActive(false);
                    imageHandler.SetActive(true);

                    StartCoroutine(ProcessImageAttribute(gameManager.questionInfos[i].questionDetails[0].media));
                }
                else
                {
                    logoTAM.SetActive(true);
                    imageHandler.SetActive(false);
                    videoHandler.SetActive(false);
                }

                soal.text = gameManager.questionInfos[i].questionDetails[0].question;
                leftAns.text = gameManager.questionInfos[i].questionDetails[0].answerDetails[0].answer;
                rightAns.text = gameManager.questionInfos[i].questionDetails[0].answerDetails[1].answer;

                if (gameManager.questionInfos[i].questionDetails[0].answerDetails[0].isCorrect)
                    StartCoroutine(SpawnAnswer(-1.4f, 1.4f));
                else
                    StartCoroutine(SpawnAnswer(1.4f, -1.4f));
            }
        }
    }

    public IEnumerator SpawnAnswer(float correct, float wrong)
    {
        yield return new WaitForSeconds(5);
        soal.text = rightAns.text = leftAns.text = string.Empty;

        Vector3 pos = new Vector3(wrong, wrongAnswer.transform.position.y, wrongAnswer.transform.position.z);
        Instantiate(wrongAnswer, pos, wrongAnswer.transform.rotation);

        pos = new Vector3(correct, wrongAnswer.transform.position.y, wrongAnswer.transform.position.z);
        Instantiate(correctAnswer, pos, correctAnswer.transform.rotation);
    }

    public IEnumerator SpawnPanel(bool cond)
    {
        int totalQuestion = 0;

        if (cond) { correctPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("CorrectSFX"); }
        else { wrongPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("WrongSFX"); }
        
        yield return new WaitForSeconds(3f);

        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);

        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                if (cond)
                    gameManager.questionInfos[i].questionDetails.RemoveAt(0);

                totalQuestion = gameManager.questionInfos[i].questionDetails.Count;
                if (totalQuestion > 0)
                    StartCoroutine(SetQuestion());
                else
                {
                    FindObjectOfType<AudioManager>().Play("GameOverSFX");

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
                        if (!isDone) gameOver[0].SetActive(true);
                        else gameOver[int.Parse(gameManager.subMasterValueId) - 1].SetActive(true);

                        StartCoroutine(gameManager.StartGame(2.5f));
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

                break;
            }
        }
    }

    IEnumerator ProcessImageAttribute(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler.GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
        imageHandler.GetComponent<Image>().preserveAspect = true;
    }
}