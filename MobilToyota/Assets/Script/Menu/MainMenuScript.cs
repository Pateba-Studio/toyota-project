using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject batasAtas;
    public GameObject batasBawah;

    public GameObject blockUser;
    public GameObject menuGame;
    public GameObject mapGame;
    public GameObject creditGame;
    public GameObject settingGame;
    public GameObject prestasiGame;
    public GameObject exitGame;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Animate Gerbang");
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        StartCoroutine(BlockUser(12));
        StartCoroutine(AnimasiMenu());
    }

    public void PilihMenu()
    {
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic();
        StartCoroutine(GasMaenGame());
    }

    public void Credit()
    {
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic();
        StartCoroutine(GasCredit());
    }

    public void Setting()
    {
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic();
        StartCoroutine(GasSetting());
    }

    public void Prestasi() {
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic();
        StartCoroutine(GasPrestasi());
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
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -575f, 1).setEaseInOutCubic();
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
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        mapGame.SetActive(true);
        menuGame.SetActive(false);
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

    IEnumerator GasPrestasi() {
        yield return new WaitForSeconds(1.2f);
        LeanTween.moveY(batasAtas.GetComponent<RectTransform>(), 1500f, 1).setEaseInOutCubic();
        LeanTween.moveY(batasBawah.GetComponent<RectTransform>(), -1500f, 1).setEaseInOutCubic();
        prestasiGame.SetActive(true);
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
        prestasiGame.SetActive(false);
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

    public GameObject _platus;
    public GameObject _startGame;
    public GameObject _infoGame;
    public GameObject _settingGame;
    public GameObject _prestasiGame;
    public GameObject _exitGame;

    IEnumerator AnimasiMenu()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_platus.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 1).setEaseInOutCubic();
        
        yield return new WaitForSeconds(1);
        LeanTween.scale(_platus.GetComponent<RectTransform>(), new Vector3(0.6f, 0.6f, 0.6f), 1).setEaseInOutCubic();
        
        yield return new WaitForSeconds(1);
        LeanTween.scale(_platus.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 1).setEaseInOutCubic();
        
        yield return new WaitForSeconds(1);
        LeanTween.moveX(_platus.GetComponent<RectTransform>(), -440, 2).setEaseInOutCubic();

        yield return new WaitForSeconds(1);
        LeanTween.scale(_startGame.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_startGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_startGame.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f).setEaseInOutCubic();
        
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_infoGame.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_infoGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_infoGame.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f).setEaseInOutCubic();
        
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_settingGame.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_settingGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_settingGame.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f).setEaseInOutCubic();
        
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_prestasiGame.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_prestasiGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_prestasiGame.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.5f).setEaseInOutCubic();

        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_exitGame.GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_exitGame.GetComponent<RectTransform>(), new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(_exitGame.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.5f).setEaseInOutCubic();
    }
}