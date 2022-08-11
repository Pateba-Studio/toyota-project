using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public RoundData[] allRoundData;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public RoundData getCurrentRoundData()
    {
        return allRoundData[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
