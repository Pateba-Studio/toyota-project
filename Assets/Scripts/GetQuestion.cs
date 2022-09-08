using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public enum HallType
{
    AB, PDP
}

[System.Serializable]
public class AssesmentStatus
{
    public bool success;
    public string assesment_status;
}

#region CheckpointClass
[System.Serializable]
public class LastCheckpoint
{
    public int sub_master_value_id;
}

[System.Serializable]
public class Datas
{
    public LastCheckpoint last_checkpoint;
}

[System.Serializable]
public class CheckpointData
{
    public bool success;
    public Datas data;
}
#endregion

#region QuestionClass
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
public class Datum
{
    public int id;
    public string sub_master_value;
    public string game_type;
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
    public List<Datum> data;
}
#endregion

#region IntroClass
[System.Serializable]
public class Data
{
    public int id;
    public string sub_master_value;
    public string media_type;
    public string media;
    public string audio;
    public string description;
}

[System.Serializable]
public class IntroData
{
    public bool success;
    public List<Data> data;
}
#endregion

#region InitializeIntro
[System.Serializable]
public class Datums
{
    public int id;
    public string master_value;
    public string media;
    public string audio;
}

[System.Serializable]
public class InitializeIntro
{
    public bool success;
    public List<Datums> data;
}
#endregion

public class GetQuestion : MonoBehaviour
{
    public HallType hallType;

    [Header("Question Hook Attribute")]
    [TextArea(2, 2)] public string hallABURL;
    [TextArea(2, 2)] public string notifAssessmentSuccessURL;
    [TextArea(2, 2)] public string assesmentURL;
    [TextArea(2, 2)] public string getAssesmentStatusURL;
    [TextArea(2, 2)] public string postCheckpointURL;
    [TextArea(2, 2)] public string getCheckpointURL;
    [TextArea(2, 2)] public string questionURL;
    [TextArea(2, 2)] public string initializeIntroURL;
    [TextArea(2, 2)] public string introURL;
    public JavascriptHook playerDataHandler;
    public IntroManager introManager;
    public GameManager gameManager;
    public AssesmentStatus assesmentStatus;
    public CheckpointData checkpointData;
    public QuestionData questionData;
    public IntroData introData;
    public InitializeIntro initializeIntro;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (playerDataHandler.isInitialized)
        {
            if (hallType == HallType.PDP)
                StartCoroutine(GetAssesmentStatus());
            else
                StartCoroutine(PostData_Coroutine());
            
            playerDataHandler.isInitialized = false;
        }
    }

    public IEnumerator PostLastCheckpoint()
    {
        WWWForm form = new WWWForm();
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        form.AddField("sub_master_value_id", playerDataHandler.playerData.sub_master_value_id);
        using (UnityWebRequest request = UnityWebRequest.Post(postCheckpointURL, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
            {
                print($"POST SUCCESS!");

                if (hallType == HallType.AB)
                    StartCoroutine(gameManager.OpenRoom(gameManager.getQuestion.hallABURL));
            }
        }
    }

    public IEnumerator GetAssesmentStatus()
    {
        WWWForm form = new WWWForm();
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        form.AddField("master_value", hallType.ToString());
        using (UnityWebRequest request = UnityWebRequest.Post(getAssesmentStatusURL, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
            {
                string jsonAssesment = request.downloadHandler.text;
                assesmentStatus = JsonUtility.FromJson<AssesmentStatus>(jsonAssesment);

                if (assesmentStatus.assesment_status == "success")
                    gameManager.assesmentIsDone = true;

                gameManager.statusIsGot = true;
                StartCoroutine(GetLastCheckpoint());
            }
        }
    }

    public IEnumerator GetLastCheckpoint()
    {
        WWWForm form = new WWWForm();
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        using (UnityWebRequest request = UnityWebRequest.Post(getCheckpointURL, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
            {
                string jsonCheckpoint = request.downloadHandler.text;
                checkpointData = JsonUtility.FromJson<CheckpointData>(jsonCheckpoint);
            }
        }

        int id = checkpointData.data.last_checkpoint.sub_master_value_id + 1;

        if (id <= 3 || id > 6 && gameManager.assesmentIsDone) id = 3;
        playerDataHandler.playerData.sub_master_value_id = $"{id}";

        if (id >= 3 && id <= 6)
            StartCoroutine(PostData_Coroutine());
    }

    public IEnumerator PostData_Coroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("sub_master_value_id", playerDataHandler.playerData.sub_master_value_id);
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        using (UnityWebRequest request = UnityWebRequest.Post(questionURL, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
            {
                string jsonQuestion = request.downloadHandler.text;
                questionData = JsonUtility.FromJson<QuestionData>(jsonQuestion);
            }
        }

        using (UnityWebRequest request = UnityWebRequest.Get($"{introURL}{playerDataHandler.playerData.sub_master_value_id}"))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                string jsonIntro = request.downloadHandler.text;
                introData = JsonUtility.FromJson<IntroData>(jsonIntro);
            }
        }

        if (gameManager.isFirstLaunched &&
            hallType == HallType.PDP)
        {
            using (UnityWebRequest request = UnityWebRequest.Get($"{initializeIntroURL}1"))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                    Debug.Log(request.error);
                else
                {
                    string jsonIntro = request.downloadHandler.text;
                    initializeIntro = JsonUtility.FromJson<InitializeIntro>(jsonIntro);
                    introManager.initializeIntro = initializeIntro;
                }
            }

            gameManager.SpawnInitializeIntro();
        }
        else if (hallType == HallType.AB)
        {
            gameManager.SetupGameManagerInAB();
        }
        else
            gameManager.SetupGameManager();
    }
}
