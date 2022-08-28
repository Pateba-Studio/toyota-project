using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;

[System.Serializable]
public class AnswerDetails
{
    public int id;
    public string answer;
    public bool isCorrect;
}

[System.Serializable]
public class QuestionDetails
{
    public int id;
    public string question;
    public string media;
    public string audio;
    public AnswerDetails[] answerDetails;
}

[System.Serializable]
public class QuestionInfo
{
    public string gameType;
    public List<QuestionDetails> questionDetails;
}

public class GameManager : MonoBehaviour
{
    public bool isPlay;
    public bool isDone;
    public bool haveGate;
    public int gameIndex;
    public float cooldownBetweenGame;
    public string subMasterValueId;
    public GameObject nextGameButton;
    public JavascriptHook javascriptHook;
    public GetQuestion getQuestion;
    public IntroManager introManager;
    public List<QuestionInfo> questionInfos;
    public static GameObject Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = gameObject;
        }
    }

    void Update()
    {
        subMasterValueId = javascriptHook.playerData.sub_master_value_id;

        if (SceneManager.GetActiveScene().name == "Gate")
        {
            introManager = GameObject.Find("Intro Manager").GetComponent<IntroManager>();
            getQuestion.introManager = introManager;

            if (getQuestion.hallType == HallType.HallPDP)
                if (int.Parse(javascriptHook.playerData.sub_master_value_id) > 6 &&
                    !introManager.assesmentPopUp.activeInHierarchy)
                    SpawnPopUpAssesment();
        }

        if (nextGameButton == null &&
            GameObject.Find("Next Game Button"))
        {
            nextGameButton = GameObject.Find("Next Game Button");
            nextGameButton.GetComponent<Button>().onClick.AddListener(() => StartGame());
        }
    }

    public void OpenAssesmentRoom()
    {
        Application.ExternalEval("window.open('" + getQuestion.assesmentURL + "','_self')");
    }

    public void SpawnPopUpAssesment()
    {
        introManager.assesmentPopUp.SetActive(true);
        introManager.assesmentPopUp.transform.GetComponentInChildren<Button>().onClick.AddListener(() => OpenAssesmentRoom());
    }

    public void SetupGameManager()
    {
        subMasterValueId = javascriptHook.playerData.sub_master_value_id;
        
        // setup questions
        for (int i = 0; i < getQuestion.questionData.data.Count; i++)
        {
            for (int j = 0; j < questionInfos.Count; j++)
            {
                if (getQuestion.questionData.data[i].game_type == questionInfos[j].gameType)
                {
                    var getQuest = getQuestion.questionData.data[i];
                    var question = new QuestionDetails();
                    
                    question.id = getQuest.id;
                    question.question = getQuest.question;
                    question.media = getQuest.media;
                    question.audio = getQuest.audio;
                    
                    questionInfos[j].questionDetails.Add(question);
                }
            }
        }
        
        // setup answers
        for (int i = 0; i < questionInfos.Count; i++)
        {
            for (int j = 0; j < questionInfos[i].questionDetails.Count; j++)
            {
                for (int k = 0; k < getQuestion.questionData.data.Count; k++)
                {
                    if (questionInfos[i].questionDetails[j].question ==
                        getQuestion.questionData.data[k].question)
                    {
                        questionInfos[i].questionDetails[j].answerDetails = new AnswerDetails[getQuestion.questionData.data[k].answers.Count];
                        for (int l = 0; l < getQuestion.questionData.data[k].answers.Count; l++)
                        {
                            questionInfos[i].questionDetails[j].answerDetails[l] = new AnswerDetails();
                            questionInfos[i].questionDetails[j].answerDetails[l].id = getQuestion.questionData.data[k].answers[l].id;
                            questionInfos[i].questionDetails[j].answerDetails[l].answer = getQuestion.questionData.data[k].answers[l].answer;
                            if (questionInfos[i].questionDetails[j].answerDetails[l].answer == getQuestion.questionData.data[k].answer_correct.answer)
                                questionInfos[i].questionDetails[j].answerDetails[l].isCorrect = true;
                        }
                    }
                }
            }
        }

        // setup intro
        for (int i = 0; i < getQuestion.introData.data.Count; i++)
        {
            var intro = new IntroInfo();
            intro.introType = getQuestion.introData.data[i].media_type;
            intro.mediaURL = getQuestion.introData.data[i].media;
            intro.audioURL = getQuestion.introData.data[i].audio;

            string desc = HTMLToText(getQuestion.introData.data[i].description);
            intro.description = desc;

            introManager.introInfo.Add(intro);
        }

        // setup gate
        for (int i = 0; i < introManager.gateDetails.Count; i++)
        {
            if (introManager.gateDetails[i].id == int.Parse(subMasterValueId))
            {
                introManager.gateDetails[i].gate.SetActive(true);
                
                if (introManager.gateDetails[i].haveGate)
                    StartCoroutine(StartGate(introManager.gateDetails[i].gate));
                else
                    introManager.introHandler.GetComponent<Animator>().SetTrigger("isPopUp");

                break;
            }
        }

        isPlay = true;
    }

    public void StartGame()
    {
        for (int i = 0; i < questionInfos.Count; i++)
        {
            if (SceneManager.GetActiveScene().name == questionInfos[i].gameType)
                questionInfos[i].questionDetails.Clear();
            if (questionInfos[i].questionDetails.Count == 0)
                isDone = true;
            else if (questionInfos[i].questionDetails.Count > 0)
            {
                isDone = false;
                break;
            }
        }

        if (!isDone)
        {
            for (int i = 0; i < questionInfos.Count; i++)
            {
                if (questionInfos[i].questionDetails.Count > 0)
                {
                    gameIndex = i;
                    SceneManager.LoadScene(questionInfos[i].gameType);
                    break;
                }
            }
        }
        else
        {
            for (int j = 0; j < questionInfos.Count; j++)
                questionInfos[j].questionDetails.Clear();

            if (getQuestion.hallType == HallType.HallPDP)
                StartCoroutine(getQuestion.PostLastCheckpoint());

            javascriptHook.playerData.sub_master_value_id = $"{int.Parse(javascriptHook.playerData.sub_master_value_id) + 1}";

            isPlay = isDone = false;
            SceneManager.LoadScene("Gate");

            if (getQuestion.hallType == HallType.HallPDP)
                if (int.Parse(javascriptHook.playerData.sub_master_value_id) <= 6)
                    StartCoroutine(getQuestion.PostData_Coroutine());
        }
    }

    public IEnumerator StartGate(GameObject gate)
    {
        yield return new WaitForSeconds(cooldownBetweenGame);
        gate.GetComponent<Animator>().SetTrigger("isPopDown");
        introManager.introHandler.GetComponent<Animator>().SetTrigger("isPopUp");
    }

    public static string HTMLToText(string HTMLCode)
    {
        // Remove new lines since they are not visible in HTML
        HTMLCode = HTMLCode.Replace("\n", " ");

        // Remove tab spaces
        HTMLCode = HTMLCode.Replace("\t", " ");

        // Remove multiple white spaces from HTML
        HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

        // Remove HEAD tag
        HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                            , RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Remove any JavaScript
        HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
          , RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Replace special characters like &, <, >, " etc.
        StringBuilder sbHTML = new StringBuilder(HTMLCode);
        // Note: There are many more special characters, these are just
        // most common. You can add new characters in this arrays if needed
        string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;","&#39;"};
        string[] NewWords = { " ", "&", "\"", "<", ">", "®", "©", "•", "™", "\'" };
        for (int i = 0; i < OldWords.Length; i++)
        {
            sbHTML.Replace(OldWords[i], NewWords[i]);
        }

        // Check if there are line breaks (<br>) or paragraph (<p>)
        sbHTML.Replace("<br>", "\n<br>");
        sbHTML.Replace("<br ", "\n<br ");
        sbHTML.Replace("<p ", "\n<p ");

        // Finally, remove all HTML tags and return plain text
        return System.Text.RegularExpressions.Regex.Replace(
          sbHTML.ToString(), "<[^>]*>", "");
    }
}
