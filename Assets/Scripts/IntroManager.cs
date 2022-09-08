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
    [TextArea(2, 2)] public string booth_type;
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
    public int introIndex;
    public GameManager gameManager;
    public GameObject assesmentPopUp;
    public GameObject generalPanel;
    public GameObject prevButton;
    public GameObject[] nextButton;
    public GameObject[] instructionText;
    public GameObject[] introHandler;
    public GameObject[] videoHandler;
    public GameObject[] imageHandler;
    public GameObject[] textHandler;
    public GameObject[] descriptionTextHandler;
    public GameObject[] titleText;
    public UnityEvent videoIsFinished;
    public InitializeIntro initializeIntro;
    public List<GateDetails> gateDetails;
    public List<IntroInfo> introInfo;

    void Awake()
    {
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        gameManager.introManager = gameObject.GetComponent<IntroManager>();

        nextButton[0].GetComponent<Button>().onClick.AddListener(() => gameManager.SetupGameManager());
        nextButton[1].GetComponent<Button>().onClick.AddListener(() => NextButtonOnClicked(1));
        
        prevButton.GetComponent<Button>().onClick.AddListener(() => NextButtonOnClicked(-1));
    }

    void Update()
    {
        if (introIndex <= 0)
            prevButton.SetActive(false);
        else
            prevButton.SetActive(true);
    }

    public void EndReached(VideoPlayer vp)
    {
        videoHandler[0].SetActive(false);
        videoHandler[1].SetActive(false);
        SetButtonAndInstruction(true, true);
    }

    public void NextButtonOnClicked(int index)
    {
        introIndex += index;
        if (introIndex >= 0 && introIndex <= introInfo.Count - 1) StartIntro();
        else StartCoroutine(gameManager.StartGame(0f));
    }

    public void StartInitializeIntro()
    {
        titleText[0].GetComponent<Text>().text = "Intro Hall PDP";

        nextButton[0].SetActive(false);
        instructionText[0].SetActive(false);

        videoHandler[0].SetActive(true);
        videoHandler[0].GetComponent<VideoScript>().videoDetails.videoURL = initializeIntro.data[0].media;
        videoHandler[0].GetComponent<VideoScript>().videoDetails.audioURL = initializeIntro.data[0].audio;

        videoIsFinished.AddListener(() => nextButton[0].SetActive(true));
        videoHandler[0].GetComponent<VideoScript>().PlayVideo(videoIsFinished);
    }

    public void StartIntro()
    {
        SetHandler(false);

        if (introInfo[introIndex].introType == "video")
        {
            SetButtonAndInstruction(false, false);

            videoHandler[1].SetActive(true);
            videoHandler[1].GetComponent<VideoScript>().videoDetails.videoURL = introInfo[introIndex].mediaURL;
            videoHandler[1].GetComponent<VideoScript>().videoDetails.audioURL = introInfo[introIndex].audioURL;

            videoIsFinished.AddListener(() => nextButton[1].SetActive(true));
            videoHandler[1].GetComponent<VideoScript>().PlayVideo(videoIsFinished);
        }
        else if (introInfo[introIndex].introType == "image")
        {
            imageHandler[1].SetActive(true);
            SetButtonAndInstruction(true, false);

            StartCoroutine(GetImage(introInfo[introIndex].mediaURL));
        }
        else if (introInfo[introIndex].introType == "text")
        {
            textHandler[1].SetActive(true);
            SetButtonAndInstruction(true, false);

            descriptionTextHandler[1].GetComponent<Text>().text = introInfo[introIndex].description;
        }
    }

    public void SetButtonAndInstruction(bool btn, bool instruct)
    {
        nextButton[1].SetActive(btn);
        instructionText[1].SetActive(instruct);
    }

    public void SetHandler(bool condition)
    {
        videoHandler[1].SetActive(condition);
        imageHandler[1].SetActive(condition);
        textHandler[1].SetActive(condition);
    }

    public IEnumerator GetImage(string mediaURL)
    {
        WWW wwwLoader = new WWW(mediaURL);
        yield return wwwLoader;

        imageHandler[1].GetComponent<Image>().sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
        imageHandler[1].GetComponent<Image>().preserveAspect = true;
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
        string[] NewWords = { " ", "&", "\"", "<", ">", "�", "�", "�", "�", "\'" };
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
