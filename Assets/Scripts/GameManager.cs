using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;
using DG.Tweening.Core.Easing;

#region QuestionClass
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
#endregion

public class GameManager : MonoBehaviour
{
    public bool isFirstLaunched;
    public bool isPlay;
    public bool isDone;
    public bool assesmentIsDone;
    public bool statusIsGot;
    public int gameIndex;
    public float cooldownBetweenGame;
    public string subMasterValueId;
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

        if (int.Parse(javascriptHook.playerData.sub_master_value_id) > 6 &&
            SceneManager.GetActiveScene().name == "Gate" &&
            getQuestion.hallType == HallType.PDP &&
            statusIsGot)
        {
            introManager = GameObject.Find("Intro Manager").GetComponent<IntroManager>();
            getQuestion.introManager = introManager;
            SpawnPopUpAssesment();
            statusIsGot = false;
        }
    }

    public IEnumerator OpenRoom(string url)
    {
        yield return new WaitForSeconds(1f);
        Application.ExternalEval("window.open('" + url + "','_self')");
    }

    public void SpawnPopUpAssesment()
    {
        introManager.assesmentPopUp.SetActive(true);
        introManager.assesmentPopUp.transform.GetComponentInChildren<Button>().onClick.AddListener(() => {
            PlayerPrefs.SetInt("PopUpAssessment", 1);
            StartCoroutine(OpenRoom(getQuestion.assesmentURL));
        });
    }

    public void SpawnInitializeIntro()
    {
        introManager.introHandler[0].GetComponent<Animator>().SetTrigger("isPopUp");
        introManager.StartInitializeIntro();
        isFirstLaunched = false;
    }

    public void SetupGameManager()
    {
        introManager.introHandler[0].GetComponent<Animator>().SetTrigger("isPopDown");
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
                {
                    introManager.titleText[1].GetComponent<TextMeshProUGUI>().text = introManager.gateDetails[i].booth_type;
                    StartCoroutine(StartGate(introManager.gateDetails[i].gate));
                }
                else
                {
                    introManager.gateDetails[i].gate.SetActive(true);
                    introManager.titleText[1].GetComponent<TextMeshProUGUI>().text = introManager.gateDetails[i].booth_type;
                    introManager.introHandler[1].GetComponent<Animator>().SetTrigger("isPopUp");
                }

                break;
            }
        }

        isPlay = true;
    }

    public void SetupGameManagerWithoutIntro()
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

        // setup gate
        for (int i = 0; i < introManager.gateDetails.Count; i++)
        {
            if (introManager.gateDetails[i].id == int.Parse(subMasterValueId))
            {
                if (introManager.gateDetails[i].haveGate)
                {
                    introManager.gateDetails[i].gate.SetActive(true);
                    introManager.titleText[1].GetComponent<TextMeshProUGUI>().text = introManager.gateDetails[i].booth_type;
                    StartCoroutine(StartGateWithoutIntro(introManager.gateDetails[i].gate));
                }
                else
                {
                    StartCoroutine(StartGame(0f));
                }

                break;
            }
        }

        isPlay = true;
    }

    public IEnumerator StartGame(float times)
    {
        yield return new WaitForSeconds(times);
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
        //else
        //{
        //    for (int j = 0; j < questionInfos.Count; j++)
        //        questionInfos[j].questionDetails.Clear();

        //    if (getQuestion.hallType == HallType.PDP)
        //    {
        //        StartCoroutine(getQuestion.PostLastCheckpoint());
        //        javascriptHook.playerData.sub_master_value_id = $"{int.Parse(javascriptHook.playerData.sub_master_value_id) + 1}";

        //        isPlay = isDone = false;
        //        SceneManager.LoadScene("Gate");

        //        if (int.Parse(javascriptHook.playerData.sub_master_value_id) <= 6)
        //            StartCoroutine(getQuestion.PostData_Coroutine());
        //    }
        //}
    }

    public IEnumerator StartGate(GameObject gate)
    {
        yield return new WaitForSeconds(cooldownBetweenGame);
        gate.GetComponent<Animator>().SetTrigger("isPopDown");
        introManager.introHandler[1].GetComponent<Animator>().SetTrigger("isPopUp");
    }

    public IEnumerator StartGateWithoutIntro(GameObject gate)
    {
        yield return new WaitForSeconds(cooldownBetweenGame);
        gate.GetComponent<Animator>().SetTrigger("isPopDown");
        yield return new WaitUntil(() => gate.transform.GetChild(1).transform.localScale.x <= 0);
        StartCoroutine(StartGame(0f));
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
        string[] NewWords = { " ", "&", "\"", "<", ">", "??", "??", "???", "???", "\'" };
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
