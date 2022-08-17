using Models;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
    public Data data;
}

public class QuestionHandler : MonoBehaviour
{
    public JavascriptHook playerDataHandler;
    public QuestionData questionData;

    string jsonQuestion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PostData_Coroutine());
    }

    IEnumerator PostData_Coroutine()
    {
        string uri = "https://tamconnect.com/api/game-question/4";
        WWWForm form = new WWWForm();
        form.AddField("ticket", playerDataHandler.playerData.ticket);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                print(request.error);
            else
                jsonQuestion = request.downloadHandler.text;
        }

        questionData = JsonUtility.FromJson<QuestionData>(jsonQuestion);
        print(questionData);
    }
}
