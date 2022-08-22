using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Soal", menuName = "Soal", order = 52)]
public class SoalData : ScriptableObject
{
    public string mediaURL;
    public string audioURL;
    public string soal;
    public List<AnswerJawaban> jawaban = new List<AnswerJawaban>();

    public string MediaURL { get { return MediaURL; } }
    public string AudioURL { get { return audioURL; } }
    public string Soal { get { return soal; } }
    public List<AnswerJawaban> Jawaban { get { return jawaban; } }

    public void SetMediaURL(string _mediaURL)
    {
        this.mediaURL = _mediaURL;
    }

    public void SetAudioURL(string _audioURL)
    {
        this.audioURL = _audioURL;
    }

    public void SetSoal(string _soal)
    {
        this.soal = _soal;
    }

    public void SetAnswer(int index, string _answer)
    {
        jawaban.Add(new AnswerJawaban(_answer, false));
    }

    public void SetCorrectAnswer(string _correctAnswer)
    {
        for (int i = 0; i < jawaban.Count; i++)
        {
            if (jawaban[i].jawaban == _correctAnswer)
                jawaban[i].isTrue = true;
            else
                jawaban[i].isTrue = false;
        }
    }
}

