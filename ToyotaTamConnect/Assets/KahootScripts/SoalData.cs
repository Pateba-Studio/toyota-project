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
    private Answer[] jawaban;
    
    
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
   
}

