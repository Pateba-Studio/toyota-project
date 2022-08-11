using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
    public string soal;
    public string plat;
    public Answers[] answers = new Answers[4];
    public Image sprite;
}
