using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGPlayAfter : MonoBehaviour
{
    public float timePlay = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (timePlay > 0)
        {
            GetComponent<AudioSource>().Stop();
            timePlay -= Time.deltaTime;
        }

        if (timePlay < 0)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
