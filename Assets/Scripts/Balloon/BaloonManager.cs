using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class BaloonManager : MonoBehaviour
{
    public bool isPlay;
    public int correctAnswer;
    public GameManager gameManager;
    public GameObject baloonGroupPrefab;
    public GameObject baloonGroup;
    public UnityEvent videoIsFinished;

    [Header("Panel Attributes")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject[] gameOver;

    [Header("Question Attributes")]
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject logoTAM;
    public Text questionText;
    public List<Text> answerText; 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        StartCoroutine(SetQuestion());
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }

    public void Play()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
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

                //    Pause();
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

                questionText.text = gameManager.questionInfos[i].questionDetails[0].question;
                for (int j = 0; j < answerText.Count; j++)
                {
                    answerText[j].text = gameManager.questionInfos[i].questionDetails[0].answerDetails[j].answer;
                    if (gameManager.questionInfos[i].questionDetails[0].answerDetails[j].isCorrect)
                        correctAnswer = j + 1;
                }
            }
        }

        baloonGroup = Instantiate(baloonGroupPrefab);
    }

    public IEnumerator PopUpHandler(bool cond, GameObject baloon)
    {
        int totalQuestion = 0;

        if (cond) { correctPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("CorrectSFX"); }
        else { wrongPanel.SetActive(true); FindObjectOfType<AudioManager>().Play("WrongSFX"); }

        isPlay = false;
        yield return new WaitForSeconds(3f);
        isPlay = true;

        Destroy(baloon);
        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);

        if (cond)
        {
            for (int i = 0; i < gameManager.questionInfos.Count; i++)
            {
                if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
                {
                    gameManager.questionInfos[i].questionDetails.RemoveAt(0);
                    totalQuestion = gameManager.questionInfos[i].questionDetails.Count;
                    break;
                }
            }

            Destroy(baloonGroup);

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
    }

    IEnumerator ProcessImageAttribute(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler.GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
        imageHandler.GetComponent<Image>().preserveAspect = true;
    }
}