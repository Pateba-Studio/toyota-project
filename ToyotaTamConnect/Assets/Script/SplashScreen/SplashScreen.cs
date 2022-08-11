using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject logo_akademik;
    public GameObject logo_game;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(LogoMuncul());
        StartCoroutine(LogoGame());
    }

    IEnumerator LogoMuncul()
    {
        yield return new WaitForSeconds(1);
        logo_akademik.SetActive(true);
        yield return new WaitForSeconds(2);
        logo_akademik.SetActive(false);
    }

    IEnumerator LogoGame()
    {
        yield return new WaitForSeconds(3);
        logo_game.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Slides");
    }
    
}
