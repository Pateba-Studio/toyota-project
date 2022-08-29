using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

[System.Serializable]
public class GateDetails
{
    public bool haveGate;
    public int id;
    public string master_value;
    public string booth_type;
    public GameObject gate;
}

[System.Serializable]
public class IntroInfo
{
    public string introType;
    public string mediaURL;
    public string audioURL;
    [TextArea(5, 5)] public string description;
}

public class IntroManager : MonoBehaviour
{
    public bool isDone;
    public GameManager gameManager;
    public GameObject assesmentPopUp;
    public GameObject generalPanel;
    public GameObject nextButton;
    public GameObject instructionText;
    public GameObject introHandler;
    public GameObject videoHandler;
    public GameObject imageHandler;
    public GameObject textHandler;
    public GameObject descriptionTextHandler;
    public UnityEvent videoIsFinished;
    public List<GateDetails> gateDetails;
    public List<IntroInfo> introInfo;

    void Awake()
    {
        nextButton.GetComponent<Button>().onClick.AddListener(() => NextButtonOnClicked());
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        gameManager.introManager = gameObject.GetComponent<IntroManager>();
    }

    void Update()
    {
        isDone = !Convert.ToBoolean(introInfo.Count);
    }

    public void EndReached(VideoPlayer vp)
    {
        videoHandler.SetActive(false);
        SetButtonAndInstruction(true, true);
    }

    public void NextButtonOnClicked()
    {
        if (!isDone) 
        { 
            StartIntro();
            introInfo.RemoveAt(0);
        }
        else StartCoroutine(gameManager.StartGame(0f));
    }

    public void StartIntro()
    {
        SetHandler(false);

        if (introInfo[0].introType == "video")
        {
            SetButtonAndInstruction(false, false);

            videoHandler.SetActive(true);
            videoHandler.GetComponent<VideoScript>().videoDetails.videoURL = introInfo[0].mediaURL;
            videoHandler.GetComponent<VideoScript>().videoDetails.audioURL = introInfo[0].audioURL;
            videoHandler.GetComponent<VideoScript>().PlayVideo(videoIsFinished);
        }
        else if (introInfo[0].introType == "image")
        {
            imageHandler.SetActive(true);
            SetButtonAndInstruction(true, false);

            StartCoroutine(GetImage(introInfo[0].mediaURL));
        }
        else if (introInfo[0].introType == "text")
        {
            textHandler.SetActive(true);
            SetButtonAndInstruction(true, false);

            descriptionTextHandler.GetComponent<Text>().text = introInfo[0].description;
        }
    }

    public void SetButtonAndInstruction(bool btn, bool instruct)
    {
        nextButton.SetActive(btn);
        instructionText.SetActive(instruct);
    }

    public void SetHandler(bool condition)
    {
        videoHandler.SetActive(condition);
        imageHandler.SetActive(condition);
        textHandler.SetActive(condition);
    }

    public IEnumerator GetImage(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler.GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
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
