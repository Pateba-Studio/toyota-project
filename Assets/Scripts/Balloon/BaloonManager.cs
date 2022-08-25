using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BaloonManager : MonoBehaviour
{
    public bool isPlay;
    public int correctAnswer;
    public float popUpTimer;
    public GameManager gameManager;
    public GameObject baloonGroupPrefab;
    public GameObject baloonGroup;
    public List<GameObject> popUpNotification;

    [Header("Text Details")]
    public Text questionText;
    public List<Text> answerText; 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Data Manager").GetComponent<GameManager>();
        SetCurrentQuestion();
    }

    public void SetCurrentQuestion()
    {
        correctAnswer = 0;
        for (int i = 0; i < gameManager.questionInfos.Count; i++)
        {
            if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name) 
            {
                questionText.text = $"{gameManager.questionInfos[i].questionDetails[0].question}";
                for (int j = 0; j < gameManager.questionInfos[i].questionDetails[0].answerDetails.Length; j++)
                {
                    answerText[j].text = gameManager.questionInfos[i].questionDetails[0].answerDetails[j].answer;
                    if (gameManager.questionInfos[i].questionDetails[0].answerDetails[j].isCorrect) correctAnswer = j + 1;
                }

                break;
            }
        }

        baloonGroup = Instantiate(baloonGroupPrefab);
    }

    public IEnumerator PopUpHandler(bool cond, GameObject baloon)
    {
        int totalQuestion = 0;

        if (cond) popUpNotification[0].SetActive(true);
        else popUpNotification[1].SetActive(true);
        
        isPlay = false;
        yield return new WaitForSeconds(popUpTimer);
        isPlay = true;

        Destroy(baloon);
        popUpNotification[0].SetActive(false);
        popUpNotification[1].SetActive(false);

        if (cond)
        {
            for (int i = 0; i < gameManager.questionInfos.Count; i++)
            {
                if (gameManager.questionInfos[i].gameType == SceneManager.GetActiveScene().name)
                {
                    gameManager.questionInfos[i].questionDetails.RemoveAt(0);
                    totalQuestion = gameManager.questionInfos[i].questionDetails.Count;
                    break;
                }
            }

            Destroy(baloonGroup);
            if (totalQuestion > 0) SetCurrentQuestion();
            else gameManager.StartGame();
        }
    }
}
