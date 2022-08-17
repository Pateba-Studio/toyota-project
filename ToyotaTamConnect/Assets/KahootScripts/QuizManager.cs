using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.Networking;

[System.Serializable]
public class Answer
{
    public int id;
    public string answer;
}

[System.Serializable]
public class AnswerCorrect
{
    public int id;
    public string answer;
}

[System.Serializable]
public class Data
{
    public int id;
    public string sub_master_value;
    public object media;
    public string media_type;
    public string question_type;
    public string question;
    public List<Answer> answers;
    public AnswerCorrect answer_correct;
}

[System.Serializable]
public class QuestionData
{
    public bool success;
    public List<Data> data;
}

public class QuizManager : MonoBehaviour
{
    private int gameScore = 0;
    public bool isWin, sfxOn;
    public Image image;
    public int jumlahSoal = 10;

    private JSONClass jsonData;
    private JSONArray jsonArray;
    private string jsonSoal, jsonA, jsonB, jsonC, jsonD, jsonCorrectAnswer;
    //[SerializeField] private int jsonId;
    public string url;
    //public int NRP;



    private string jsonQuestion;

    //[SerializeField] private float time;
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
    public JavascriptHook playerDataHandler;
    public QuestionData questionData;
    public SoalData[] questions;

    private void Awake()
    {
        //StartCoroutine(LoadJson(url));
        StartCoroutine(PostData_Coroutine());
    }
    void Start()
    {
        //GetQuestion();
        sfxOn = true;
        //if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        //{
        //    unansweredQuestions = questions.ToList<SoalData>();
        //}
        
        //SetCurrentQuestion();
    }

    void StartQuestion()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<SoalData>();
        }
        SetCurrentQuestion();
    }
    private IEnumerator LoadJson(string urlString)
    {
        string link = urlString;
        WWW jsonUrl = new WWW(link);

        yield return jsonUrl;

        if (jsonUrl.error == null)
        {
            
            //GetQuestion(quest);
            //SetJSONData(jsonId, jsonName, jsonEmail, jsonAvatar);
        }
        else
        {
            print("ERROR : " + jsonUrl.error);
        }
    }
    

    
      
   

    IEnumerator PostData_Coroutine()
    {
        string uri = "https://tamconnect.com/api/game-question";
        WWWForm form = new WWWForm();
        form.AddField("sub_master_value_id", playerDataHandler.playerData.sub_master_value_id);
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
            {
                jsonQuestion = request.downloadHandler.text;
                GetQuestion(jsonQuestion);
            }
                
            
        }

        
        //GetQuestion(questionData);
        //print(questionData);
    }
    private void SetJSONData(int _id, string _name, string _email, string _avatar)
    {
        jsonData = new JSONClass(_id, _name, _email, _avatar);
    }

    void Update()
    {
        if (!isWin)
        {
            //time += Time.deltaTime;

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
                FindObjectOfType<AudioManager>().Play("GameOverSFX");
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

        StartCoroutine(TutupSoal());

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
            foreach (AnswerJawaban jawaban in currentQuestion.Jawaban)
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
        StartQuestion();
        
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


    public void GetQuestion(string jsonData)
    {
        //jsonArray = JSON.Parse(jsonData).AsArray;
        questionData = JsonUtility.FromJson<QuestionData>(jsonQuestion);
        questions = new SoalData[questionData.data.Count];
        jumlahSoal = questions.Length;

        for (int i = 0; i < questionData.data.Count; i++)
        {
            jsonSoal = questionData.data[i].question;
            jsonA = questionData.data[i].answers[0].answer;
            jsonB = questionData.data[i].answers[1].answer;
            jsonC = questionData.data[i].answers[2].answer;
            jsonD = questionData.data[i].answers[2].answer;
            jsonCorrectAnswer = questionData.data[i].answer_correct.answer;

            //print("Perkenalkan, nama saya " + jsonSoal + ". Anda bisa menghubungi saya melalui email dibawah ini " + jsonC);
            SetQuestion(i, jsonSoal, jsonA, jsonB, jsonC, jsonD, jsonCorrectAnswer);
            
        }

        StartQuestion();

    }
    public void SetQuestion(int counter, string _soal, string _a, string _b , string _c, string _d, string _correctAnswer)
    {
        //jsonData = new JSONClass(_id, _name, _email, _avatar);
        SoalData s = ScriptableObject.CreateInstance<SoalData>();
        s.SetSoal(_soal);
       
        s.SetAnswerA(_a, false);
        s.SetAnswerB(_b, false);
        s.SetAnswerC(_c, false);
        s.SetAnswerD(_d, false);
        s.SetCorrectAnswer(_correctAnswer);
        //s.SetImage()


        //print("Soal ke " + s.Soal);
        questions[counter] = s;

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
