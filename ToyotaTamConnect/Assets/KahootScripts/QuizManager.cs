using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class QuizManager : MonoBehaviour
{
   
    private int gameScore = 0;
    //private float time;
    public bool isWin, sfxOn;
    public Image image;
    public int jumlahSoal = 10;

    private JSONClass jsonData;
    private JSONArray jsonArray;
    [SerializeField] private string jsonName, jsonAvatar;
    [SerializeField] private int jsonId;
    public string url;
    public int NRP;

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
    [SerializeField] private Button[] answerButton;
    public SoalData[] questions;

    void Start()
    {
        //GetQuestion();

        sfxOn = true;
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<SoalData>();
        }

       
        SetCurrentQuestion();
        //StartCoroutine(LoadJson(url));
    }
    private IEnumerator LoadJson(string urlString)
    {
        string link = urlString;
        WWW jsonUrl = new WWW(link);

        yield return jsonUrl;

        if (jsonUrl.error == null)
        {
            
            //GetQuestion(jsonUrl.text);
            //SetJSONData(jsonId, jsonName, jsonEmail, jsonAvatar);
        }
        else
        {
            print("ERROR : " + jsonUrl.error);
        }
    }
    private void GetJSONData(string jsonData)
    {
        jsonArray = JSON.Parse(jsonData).AsArray;

        jsonName = jsonArray[18]["name"];
    }
    private void SetJSONData(int _id, string _name, string _email, string _avatar)
    {
        jsonData = new JSONClass(_id, _name, _email, _avatar);
    }

    void Update()
    {
        if (!isWin)
        {
            time += Time.deltaTime;

        }

        if (!isWin && soalCounter == questions.Length)
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

        EnableAnswer();
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
            soalCounter++;

            StartCoroutine(CorrectAnswer());
            StartCoroutine(TransitionToNextQuestion());
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
            DisableAnswer(i);
            StartCoroutine(WrongAnswer());
        }

        if (!isWin)
        {
            if (soalCounter != jumlahSoal)
            {
               // StartCoroutine(TransitionToNextQuestion());
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
        FindObjectOfType<AudioManager>().Play("CorrectSFX");
        yield return new WaitForSeconds(timeBetweenQuestions);
        correctPanel.SetActive(false);
    }

    private IEnumerator WrongAnswer()
    {
        wrongPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("WrongSFX");
        yield return new WaitForSeconds(timeBetweenQuestions);
        wrongPanel.SetActive(false);
    }

    private void DisableAnswer(int i)
    {
        answerButton[i].interactable = false;
    }

    private void EnableAnswer()
    {
        for(int i = 0; i < 4; i++)
        {
            //answerButton[i].interactable = false;
            answerButton[i].interactable = true;
        }
    }
    private IEnumerator ToGameOver()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        isWin = true;
    }


    public void GetQuestion()
    {
        //jsonArray = JSON.Parse(jsonData).AsArray;
        //int i = 1;
        //int j = 0;

        for(int i = 1; i <= jumlahSoal; i++)
        {
            SoalData s = ScriptableObject.CreateInstance<SoalData>();
            s.SetSoal("hayuk");
            s.SetAnswerA("benar", true);
            s.SetAnswerB("salah", false);
            s.SetAnswerC("wrong", false);
            s.SetAnswerD("false", false);

            questions = new SoalData[i];

            questions[i-1] = s;

        }

    }

    private int GetId()
    {
        return jsonData.id;
    }
    private string GetName()
    {
        return jsonData.name;
    }
    private string GetEmail()
    {
        return jsonData.email;
    }
    private string GetAvatar()
    {
        return jsonData.avatar;
    }

}
