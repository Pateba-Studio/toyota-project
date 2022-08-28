using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public enum HallType
{
    HallAB, HallPDP
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

public class GetQuestion : MonoBehaviour
{
    public HallType hallType;

    [Header("Question Hook Attribute")]
    [TextArea(2, 2)] public string assesmentURL;
    [TextArea(2, 2)] public string postCheckpointURL;
    [TextArea(2, 2)] public string getCheckpointURL;
    [TextArea(2, 2)] public string questionURL;
    [TextArea(2, 2)] public string introURL;
    public JavascriptHook playerDataHandler;
    public IntroManager introManager;
    public GameManager gameManager;
    public CheckpointData checkpointData;
    public QuestionData questionData;
    public IntroData introData;

    string jsonCheckpoint;
    string jsonQuestion;
    string jsonIntro;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (playerDataHandler.isInitialized)
        {
            if (hallType == HallType.HallPDP)
                StartCoroutine(PostLastCheckpoint());
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
                jsonCheckpoint = request.downloadHandler.text;
                checkpointData = JsonUtility.FromJson<CheckpointData>(jsonCheckpoint);
            }
        }

        int id = checkpointData.data.last_checkpoint.sub_master_value_id + 1;

        if (id < 3) id = 3;
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
                jsonQuestion = request.downloadHandler.text;
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
                jsonIntro = request.downloadHandler.text;
                introData = JsonUtility.FromJson<IntroData>(jsonIntro);
            }
        }

        gameManager.SetupGameManager();
    }
}
