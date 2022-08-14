using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Soal", menuName = "Soal", order = 52)]
public class SoalData : ScriptableObject
{
    [SerializeField]
    private string soal;

    [SerializeField]
    private Answer[] jawaban = new Answer[4];
    
    
    [SerializeField]
    private Sprite image;




    public string Soal
    {
        get
        {
            return soal;
        }
    }

    public Answer[] Jawaban
    {
        get
        {
            return jawaban;
        }
    }

    public Sprite Image
    {
        get
        {
            return image;
        }
    }


    public void SetSoal(string _soal)
    {
        this.soal = _soal;
    }
    public void SetAnswerA(string _answer, bool _isCorrect)
    {
        jawaban[0] =  new Answer(_answer, _isCorrect);
    }
    public void SetAnswerB(string _answer, bool _isCorrect)
    {
        jawaban[1] = new Answer(_answer, _isCorrect);
    }
    public void SetAnswerC(string _answer, bool _isCorrect)
    {
        jawaban[2] = new Answer(_answer, _isCorrect);
    }
    public void SetAnswerD(string _answer, bool _isCorrect)
    {
        jawaban[3] = new Answer(_answer, _isCorrect);
    } 
    public void SetImage(Sprite _image)
    {
        this.image = _image;
    }

    public void SetCorrectAnswer(string _correctAnswer)
    {
        foreach (Answer jawaban in jawaban)
        {
            if (jawaban.jawaban == _correctAnswer)
            {
                jawaban.isTrue = true;
            }
            else 
            { 
                jawaban.isTrue = false; 
            }
        }
    }
    

}

