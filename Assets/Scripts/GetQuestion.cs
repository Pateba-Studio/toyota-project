using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    [Header("Question Hook Attribute")]
    [TextArea(2, 2)] public string questionURL;
    [TextArea(2, 2)] public string introURL;
    public JavascriptHook playerDataHandler;
    public IntroManager introManager;
    public GameManager gameManager;
    public QuestionData questionData;
    public IntroData introData;

    string jsonQuestion;
    string jsonIntro;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
