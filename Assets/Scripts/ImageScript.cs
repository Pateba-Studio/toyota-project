using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public GameObject imageFullScreen;

    public void SetImageFullScreen()
    {
        if (!imageFullScreen.activeInHierarchy)
        {
            imageFullScreen.SetActive(true);
            imageFullScreen.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
            imageFullScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(SetImageNormalSize);
            Time.timeScale = 0;
        }
    }

    public void SetImageNormalSize()
    {
        if (imageFullScreen.activeInHierarchy)
        {
            Time.timeScale = 1;
            imageFullScreen.SetActive(false);
        }
    }
}