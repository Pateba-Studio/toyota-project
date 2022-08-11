using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBackScript : MonoBehaviour
{
    public GameObject batasAtas;
    public GameObject batasBawah;

    public GameObject blockUser;
    public GameObject menuGame;
    public GameObject mapGame;
    public GameObject creditGame;
    public GameObject settingGame;
    public GameObject exitGame;

    public void PilihMenu()
    {
        /* batasAtas.GetComponent<Animator>().SetTrigger("batasAtas");
        batasBawah.GetComponent<Animator>().SetTrigger("batasBawah"); */
        SceneManager.LoadScene("MapScene");
    }

    public void Credit()
    {
        /* LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic(); */
        StartCoroutine(GasCredit());
    }

    public void Setting()
    {
        /* LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic(); */
        StartCoroutine(GasSetting());
    }

    public void MetuGame()
    {
        blockUser.SetActive(true);
        StartCoroutine(BlockUser(2.5f));
        StartCoroutine(MetuTekanGame());
    }

    public void IyoMetu()
    {
        Application.Quit(0);
    }

    public void OraMetu()
    {
        LeanTween.scale(exitGame.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }

    public void Home()
    {
        /* LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic(); */
        StartCoroutine(MbalekOmah());
    }

    IEnumerator BlockUser(float time)
    {
        yield return new WaitForSeconds(time);
        blockUser.SetActive(false);
    }

    IEnumerator GasMaenGame()
    {
        yield return new WaitForSeconds(1.2f);
        
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator GasCredit()
    {
        yield return new WaitForSeconds(1.2f);
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        creditGame.SetActive(true);
        menuGame.SetActive(false);
    }

    IEnumerator GasSetting()
    {
        yield return new WaitForSeconds(1.2f);
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        settingGame.SetActive(true);
        menuGame.SetActive(false);
    }

    IEnumerator MetuTekanGame()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(exitGame.GetComponent<RectTransform>(), new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(exitGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(exitGame.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f).setEaseInOutCubic();
    }

    IEnumerator MbalekOmah()
    {
        yield return new WaitForSeconds(1.2f);
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        menuGame.SetActive(true);
        creditGame.SetActive(false);
        settingGame.SetActive(false);
        mapGame.SetActive(false);
    }
    
    public GameObject jawaBarat;
    public GameObject jawaTengah;
    public GameObject jawaTimur;

    public void PoskoJabar()
    {
        LeanTween.scale(jawaBarat.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }

    public void YoJabar()
    {
        //SceneManager_MiniGame_Dicky
        SceneManager.LoadScene("PlatformMinigame");
    }

    public void GakJabar()
    {
        LeanTween.scale(jawaBarat.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }

    public void PoskoJateng()
    {
        LeanTween.scale(jawaTengah.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }
    
    public void YoJateng()
    {
        SceneManager.LoadScene("MontorMinigame");
    }

    public void GakJateng()
    {
        LeanTween.scale(jawaTengah.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }

    public void PoskoJatim()
    {
        LeanTween.scale(jawaTimur.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 1).setEaseInOutCubic();
    }
    
    public void YoJatim()
    {
        //SceneManager_MiniGame_Arlo
        SceneManager.LoadScene("QuizMinigame");
    }

    public void GakJatim()
    {
        LeanTween.scale(jawaTimur.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 1).setEaseInOutCubic();
    }
}
