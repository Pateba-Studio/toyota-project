using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
    public string media;
    public string audio;
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
    public bool isWin, sfxOn;

    [Header("Question Hook Attribute")]
    public string url;
    public JavascriptHook playerDataHandler;
    public QuestionData questionData;

    [Header("Question Display Attribute")]
    public int jumlahSoal;
    public int soalCounter;
    public float timeBetweenQuestions;
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject logoTAM;
    public TextMeshProUGUI soalText;
    public TextMeshProUGUI[] answerText;
    public SoalData[] questions;
    public Button[] answerButton;

    [Header("Panel Attribute")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject gameOverPanel;

    SoalData currentQuestion;
    public List<SoalData> unansweredQuestions;
    string jsonQuestion;

    void Awake()
    {
        StartCoroutine(PostData_Coroutine());
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

            gameOverPanel.SetActive(true);
            gameObject.SetActive(false);
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

    void VideoEndReached(VideoPlayer vp)
    {
        logoTAM.SetActive(true);
        videoHandler.SetActive(false);

        for (int i = 0; i < answerButton.Length; i++)
            answerButton[i].interactable = true;
    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        soalText.text = currentQuestion.Soal;

        if (currentQuestion.mediaURL != string.Empty &&
            currentQuestion.audioURL != string.Empty)
        {
            logoTAM.SetActive(false);
            videoHandler.SetActive(true);

            videoHandler.GetComponent<RawImage>().color = new Color32(0, 0, 0, 0);
            videoHandler.transform.GetChild(0).gameObject.SetActive(true);

            for (int i = 0; i < answerButton.Length; i++)
                answerButton[i].interactable = false;

            StartCoroutine(ProcessVideoAttribute(currentQuestion.mediaURL, currentQuestion.audioURL));
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

    public void GetQuestion(string jsonData)
    {
        questionData = JsonUtility.FromJson<QuestionData>(jsonQuestion);
        questions = new SoalData[questionData.data.Count];
        jumlahSoal = questions.Length;

        for (int i = 0; i < questionData.data.Count; i++)
        {
            string mediaURL = questionData.data[i].media;
            string audioURL = questionData.data[i].audio;
            string jsonSoal = questionData.data[i].question;
            List<string> jsonAnswer = new List<string>();

            for (int j = 0; j < questionData.data[i].answers.Count; j++)
            {
                jsonAnswer.Add(questionData.data[i].answers[j].answer);
            }

            string jsonCorrectAnswer = questionData.data[i].answer_correct.answer;
            SetQuestion(i, mediaURL, audioURL, jsonSoal, jsonAnswer, jsonCorrectAnswer);
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
        if (currentQuestion.Jawaban[i].isTrue)
        {
            isWin = false;
            soalCounter++;

            StartCoroutine(CorrectAnswer());
            StartCoroutine(TransitionToNextQuestion());

            logoTAM.SetActive(true);
            videoHandler.SetActive(false);
            imageHandler.SetActive(false);
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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PostData_Coroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("sub_master_value_id", playerDataHandler.playerData.sub_master_value_id);
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
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
        yield return new WaitForSeconds(timeBetweenQuestions);
        correctPanel.SetActive(false);
    }

    IEnumerator WrongAnswer()
    {
        wrongPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("WrongSFX");
        yield return new WaitForSeconds(timeBetweenQuestions);
        wrongPanel.SetActive(false);
    }

    IEnumerator ToGameOver()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        isWin = true;
    }

    IEnumerator ProcessVideoAttribute(string mediaURL, string audioURL)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
                Debug.Log(www.error);
            else
            {
                videoHandler.GetComponent<VideoPlayer>().url = mediaURL;
                videoHandler.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);

                videoHandler.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;
                videoHandler.GetComponent<VideoPlayer>().EnableAudioTrack(0, false);
                videoHandler.GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);

                videoHandler.GetComponent<VideoPlayer>().Play();
                videoHandler.GetComponent<AudioSource>().Play();

                videoHandler.transform.GetChild(0).gameObject.SetActive(false);
                videoHandler.GetComponent<RawImage>().color = new Color32(255, 255, 225, 225);

                videoHandler.GetComponent<VideoPlayer>().loopPointReached += VideoEndReached;
            }
        }
    }

    IEnumerator ProcessImageAttribute(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler.GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
    }
}
