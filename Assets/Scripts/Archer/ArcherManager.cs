using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class ArcherManager : MonoBehaviour
{
    public bool isDone;
    public GameManager gameManager;
    public GameObject arrowObject;
    public BowController bowController;
    public ArrowAnimation arrowAnimation;
    public UnityEvent videoIsFinished;

    [Header("Panel Attributes")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject[] gameOver;

    [Header("Question Attributes")]
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject logoTAM;
    public TextMeshProUGUI[] answers;
    public TextMeshProUGUI soal;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        StartCoroutine(SetQuestion());
    }

    public IEnumerator SetQuestion()
    {
        logoTAM.SetActive(false);
        imageHandler.SetActive(false);
        videoHandler.SetActive(false);

        yield return null;

        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                //if (gameManager.questionInfos[i].questionDetails[0].media != string.Empty &&
                //    gameManager.questionInfos[i].questionDetails[0].audio != string.Empty)
                //{
                //    logoTAM.SetActive(false);
                //    videoHandler.SetActive(true);

                //    videoHandler.GetComponent<VideoScript>().videoDetails.videoURL = gameManager.questionInfos[i].questionDetails[0].media;
                //    videoHandler.GetComponent<VideoScript>().videoDetails.audioURL = gameManager.questionInfos[i].questionDetails[0].audio;
                //    videoHandler.GetComponent<VideoScript>().PlayVideo(videoIsFinished);

                //    bowController.enabled = false;
                //    arrowAnimation.enabled = false;
                //}
                //else if (gameManager.questionInfos[i].questionDetails[0].media != string.Empty)
                //{
                //    logoTAM.SetActive(false);
                //    imageHandler.SetActive(true);

                //    StartCoroutine(ProcessImageAttribute(gameManager.questionInfos[i].questionDetails[0].media));
                //}
                //else
                //{
                //    logoTAM.SetActive(true);
                //    imageHandler.SetActive(false);
                //    videoHandler.SetActive(false);
                //}

                logoTAM.SetActive(false);
                imageHandler.SetActive(false);
                videoHandler.SetActive(false);

                soal.text = gameManager.questionInfos[i].questionDetails[0].question;

                for (int j = 0; j < answers.Length; j++)
                {
                    answers[j].text = gameManager.questionInfos[i].questionDetails[0].answerDetails[j].answer;
                }
            }
        }
    }

    public IEnumerator SpawnPanel(bool cond)
    {
        int totalQuestion = 0;

        if (cond) { correctPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("CorrectSFX"); }
        else { wrongPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("WrongSFX"); }

        bowController.isPlay = false;
        bowController.touchStart = false;
        arrowAnimation.enabled = false;

        yield return new WaitForSeconds(3f);

        bowController.isPlay = true;
        bowController.touchStart = true;
        arrowAnimation.enabled = true;

        arrowObject.transform.position = new Vector3(0, 0, 0);
        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);

        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                if (cond)
                {
                    gameManager.questionInfos[i].questionDetails.RemoveAt(0);

                    totalQuestion = gameManager.questionInfos[i].questionDetails.Count;
                    if (totalQuestion > 0)
                        StartCoroutine(SetQuestion());
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("GameOverSFX");

                        bool isFinish = false;
                        for (int j = 0; j < gameManager.questionInfos.Count; j++)
                        {
                            if (gameManager.questionInfos[j].questionDetails.Count > 0)
                            {
                                isFinish = false;
                                break;
                            }
                            else
                                isFinish = true;
                        }

                        if (gameManager.getQuestion.hallType == HallType.PDP)
                        {
                            if (!isFinish)
                            {
                                gameOver[0].SetActive(true);
                                StartCoroutine(gameManager.StartGame(2.5f));
                            }
                            else if (isFinish && !isDone)
                            {
                                gameOver[int.Parse(gameManager.subMasterValueId) - 1].SetActive(true);
                                StartCoroutine(gameManager.getQuestion.PostLastCheckpoint());
                                isDone = true;
                            }
                        }
                        else
                        {
                            if (!isFinish)
                            {
                                gameOver[0].SetActive(true);
                                StartCoroutine(gameManager.StartGame(2.5f));
                            }
                            else if (isFinish && !isDone)
                            {
                                gameOver[1].SetActive(true);
                                StartCoroutine(gameManager.getQuestion.PostLastCheckpoint());
                                //StartCoroutine(gameManager.OpenRoom(gameManager.getQuestion.hallABURL));
                                isDone = true;
                            }
                        }
                    }
                }
                break;
            }
        }
    }

    public void ChooseAnswer(int answer)
    {
        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
            {
                if (gameManager.questionInfos[i].questionDetails[0].answerDetails[answer].isCorrect)
                {
                    StartCoroutine(SpawnPanel(true));
                    bowController.enabled = true;
                }
                else
                {
                    StartCoroutine(SpawnPanel(false));
                    bowController.enabled = true;
                }
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
