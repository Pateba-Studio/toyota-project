using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

// ARLO MARIO DENDI
// 4210181018
public class BubbleChat : MonoBehaviour
{
    public string url;
    public int NRP;
    private JSONClass jsonData;
    private JSONArray jsonArray;
    [SerializeField] private string jsonName, jsonEmail, jsonAvatar;
    [SerializeField] private int jsonId;
    //public GameObject chatBubble;
    //public TextMeshProUGUI chat;

    void Start()
    {
        StartCoroutine(LoadJson(url));
        //chatBubble.SetActive(false);
    }
    private IEnumerator LoadJson(string urlString)
    {
        string link = urlString;
        WWW jsonUrl = new WWW(link);

        yield return jsonUrl;

        if (jsonUrl.error == null)
        {
            //chatBubble.SetActive(true);
            GetJSONData(jsonUrl.text);
            //print(jsonName);
            SetJSONData(jsonId, jsonName, jsonEmail, jsonAvatar);
            SetChatBubbleText();
        }
        else
        {
            print("ERROR : " + jsonUrl.error);
        }
    }

    private void GetJSONData(string jsonData)
    {
        jsonArray = JSON.Parse(jsonData).AsArray;

        //NRP = 4210181018
        jsonId = jsonArray[NRP]["id"];
        jsonName = jsonArray[18]["name"];
        jsonEmail = jsonArray[18]["email"];
        jsonAvatar = jsonArray[NRP]["avatar"];
    }

    private void SetJSONData(int _id, string _name, string _email, string _avatar)
    {
        jsonData = new JSONClass(_id, _name, _email, _avatar);
    }

    private void SetChatBubbleText()
    {
        //chat.text = "Perkenalkan, nama saya " + GetName() + ". Anda bisa menghubungi saya melalui email dibawah ini " + GetEmail();
        print("Perkenalkan, nama saya " + GetName() + ". Anda bisa menghubungi saya melalui email dibawah ini " + GetEmail());
        
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
