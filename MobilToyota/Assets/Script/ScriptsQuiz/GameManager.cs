using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    public GameObject soalPanel, PauseButton, popUpTrue, popUpFalse, popUpTimeUp, popWin, popLose, popfive, losefive,popUpAch, popCardPanel, medalObj, tingkatkanSkor;
    private int gameScore = 0;
    private float time;
    public string[] kotaNama;
    public bool isWin;
    private int persentase;

    public bool isGetCard;

    private static List<Question> unansweredQuestions;

    private Question currentQuestion;

    [SerializeField] Sprite[] cardSpriteList, medalSpriteList;

    [SerializeField] Image popCard;

    [SerializeField] private TextMeshProUGUI soalText;

    [SerializeField] private TextMeshProUGUI platText;

    [SerializeField] private TextMeshProUGUI achText;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private TextMeshProUGUI kotaKanan;

    [SerializeField] private TextMeshProUGUI kotaKiri;

    [SerializeField] private float timeBetweenQuestions = 1f;

    [SerializeField] private TextMeshProUGUI[] answerText;



    void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        SetCurrentQuestion();
        SetRandomKota();
    }

    void Update()
    {
      
        if (ClickManager.cdStart)
        {
            ClickManager.cdStart = false;
            StartCoroutine(StartCountdown(60)); // WAKTU MENJAWAB
        }

        if (isWin) 
        {
            isWin = false;
            //StartCoroutine(GameOverAchievement(1f));
        }

    }
    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        soalText.text = currentQuestion.soal;
        platText.text = currentQuestion.plat;

        for (int i=0; i < 4;i++)
        {
            answerText[i].text = currentQuestion.answers[i].jawaban;
        }
    }
    public IEnumerator StartCountdown(float countdownValue)
    {
        time = countdownValue;
        while (time > 0)
        {
            //Debug.Log("Countdown: " + time);
            yield return new WaitForSeconds(1.0f);
            time--;
            
            if(time <= 0 && !isWin)
            {
                isWin = true;
                time = 0;
                ClickManager.cdStart = false;
                Debug.Log("WAKTU HABIS");
                soalPanel.GetComponent<Tweener>().OnClose();

                popUpTimeUp.SetActive(true);
                popUpTimeUp.GetComponent<Tweener>().TimeUp();


                kotaKanan.GetComponent<Tweener>().OnClose();
                kotaKiri.GetComponent<Tweener>().OnClose();

                StartCoroutine(GameOverAchievement(1f)); // POP UP KALAH (RESTART HOME)
            
            }
            timerText.text = time.ToString();
        }   
    }

    IEnumerator GameOver(float time)
    {
        
        yield return new WaitForSeconds(time);
        //PauseButton.GetComponent<Button>().onClick.Invoke();

    }

    IEnumerator GameOverAchievement(float time)
    {
        CheckPercentage(gameScore);
        yield return new WaitForSeconds(time);
        //popWin.SetActive(true);
        //popWin.GetComponent<Tweener>().PopUp();

        

        //achText.text = "Skor mu " + gameScore; 

        //POPUP ACHIEVEMENT

        //popUpAch.SetActive(true);
        //popUpAch.GetComponent<Tweener>().PopUp();
        //Debug.Log("ACHIEVEMENT");

        //PauseButton.GetComponent<Button>().onClick.Invoke();

    }
    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);
        yield return new WaitForSeconds(timeBetweenQuestions);
        
        StartCoroutine("TutupSoal");
    }

    public void UserSelectTrue(int i)
    {
        if (currentQuestion.answers[i].isTrue)
        {
            //Debug.Log("CORRECT");
            gameScore += 10;
            scoreText.text = "" + gameScore.ToString();
            popUpTrue.SetActive(true);
            popUpTrue.GetComponent<Tweener>().PopUp();

            popfive.SetActive(true);
            popfive.GetComponent<Tweener>().PopUp();

            kotaKanan.GetComponent<Tweener>().OnClose();
            kotaKiri.GetComponent<Tweener>().OnClose();


            //if (gameScore == 100 && !isWin) //BATAS SKOR
            //{
            //    isWin = true;
            //    Debug.Log("GANTI LEVEL"); // POP UP 

            //    popUpTrue.SetActive(true);
            //    popUpTrue.GetComponent<Tweener>().PopUp();

            //    popfive.SetActive(true);
            //    popfive.GetComponent<Tweener>().PopUp();

               

            //}
        }
        else
        {
            isWin = false;
            //Debug.Log("WRONG");
            //time -= 1;
            popUpFalse.SetActive(true);
            popUpFalse.GetComponent<Tweener>().PopUp();

            //losefive.SetActive(true);
            //losefive.GetComponent<Tweener>().PopUp();


            kotaKanan.GetComponent<Tweener>().OnClose();
            kotaKiri.GetComponent<Tweener>().OnClose();
        }
        
        StartCoroutine(TransitionToNextQuestion());
    }

    private IEnumerator TutupSoal()
    {
        yield return new WaitForSeconds(0.3f);
        PauseButton.SetActive(true);
        kotaKanan.gameObject.SetActive(true);
        kotaKiri.gameObject.SetActive(true);
        Start();
    }

    public void ButtonClickA()
    {
        UserSelectTrue(0);
    }
    public void ButtonClickB()
    {
        UserSelectTrue(1);
    }
    public void ButtonClickC()
    {
        UserSelectTrue(2);
    }
    public void ButtonClickD()
    {
        UserSelectTrue(3);
    }

    public void ButtonClickX()
    {
        StartCoroutine("TutupSoal");
    }

    void SetRandomKota()
    {
        int i = Random.Range(0, 27);
        int j = Random.Range(0, 27);
        if(i == j)
        {
            SetRandomKota();
        }
        kotaKanan.text = kotaNama[i].ToString();
        kotaKiri.text = kotaNama[j].ToString();
    }

    
    void CheckPercentage(int score)
    {
        if (score >= 10 && score <= 30)
        {
            SetMedal(2);
            Debug.Log("PERUNGGU 33.3 %");
            if (Random.Range(0, 3) == 2) //33,3% dapet card jatim?
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        DapetKartuJatim(1);
                        break;
                    case 1:
                        DapetKartuJatim(2);
                        break;
                }
            }
            else
                isGetCard = false;
        }

        if (score >= 40 && score <= 60)
        {
            SetMedal(1);
            Debug.Log("PERAK 66.6 %");
            if (Random.Range(0, 3) >= 1) //66,6% dapet card jatim?
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        DapetKartuJatim(1);
                        break;
                    case 1:
                        DapetKartuJatim(2);
                        break;
                }
            }
            else
                isGetCard = false;
        }

        if (score >= 70)
        {
            SetMedal(0);
            Debug.Log("EMAS 100 %"); 
            switch (Random.Range(0, 2)) //100% dapet card random jatim
            {
                case 0:
                    DapetKartuJatim(1);
                    break;
                case 1:
                    DapetKartuJatim(2);
                    break;
            }
        }
        if (score == 0)
            tingkatkanSkor.SetActive(true);

        achText.text = gameScore.ToString();
    }

    void SetMedal(int medalNum)
    {
        medalObj.GetComponent<Image>().sprite = medalSpriteList[medalNum];
        medalObj.SetActive(true);
    }
    private void DapetKartuJatim(int number)
    {
        popCard.sprite = cardSpriteList[number - 1];
        popCardPanel.SetActive(true);
        PlayerPrefs.SetInt("Jatim" + number, 1);
        Debug.Log("dpt Jatim " + number);
    }

    public void OKCardButton()
    {
        if(isGetCard == false)
        {
            Debug.Log("tidak dapet kartu");
            SceneManager.LoadScene("Menu");
        }

        if(isGetCard == true)
        {
            Debug.Log("dapet kartu");
            popCardPanel.SetActive(true);
            //popCardPanel.GetComponent<Tweener>().PopCard();
        }
    }

}
 