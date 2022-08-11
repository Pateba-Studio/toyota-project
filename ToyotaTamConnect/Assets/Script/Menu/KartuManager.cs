using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartuManager : MonoBehaviour
{
    [SerializeField] GameObject[] kartuBuka;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("Jateng1", 0); //ngetes reset
        //PlayerPrefs.SetInt("Jateng2", 0);
        //PlayerPrefs.SetInt("Jatim1", 0); //ngetes reset
        //PlayerPrefs.SetInt("Jatim2", 0);
        if (PlayerPrefs.GetInt("Jabar1") == 1)
            kartuBuka[0].SetActive(true);

        if (PlayerPrefs.GetInt("Jabar2") == 1)
            kartuBuka[1].SetActive(true);
        
        if (PlayerPrefs.GetInt("Jateng1") == 1)
            kartuBuka[2].SetActive(true);
        
        if (PlayerPrefs.GetInt("Jateng2") == 1)
            kartuBuka[3].SetActive(true);
        
        if (PlayerPrefs.GetInt("Jatim1") == 1)
            kartuBuka[4].SetActive(true);
        
        if (PlayerPrefs.GetInt("Jatim2") == 1)
            kartuBuka[5].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
