using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tweener : MonoBehaviour
{
    public UnityEvent onCompleteCallBack;
    public void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setDelay(0.5f).setOnComplete(OnComplete).setIgnoreTimeScale(true);
    }

    public void PopUp()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setDelay(1f).setOnComplete(OnClose).setIgnoreTimeScale(true); ;
    }

    public void PopCard()
    {
        transform.localScale = new Vector3(2.35f, 2.35f, 2.35f);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setDelay(1f).setOnComplete(OnClose).setIgnoreTimeScale(true); ;
    }
   
    public void TimeUp()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setOnComplete(OnCloseTime).setIgnoreTimeScale(true); ;
        
    }

    public void WinUp()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f).setOnComplete(OnCloseWin).setIgnoreTimeScale(true); ;
    }

    public void OnCloseWin()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setDelay(1f).setOnComplete(OnComplete).setIgnoreTimeScale(true); ;
    }


    public void OnComplete()
    {
        if(onCompleteCallBack != null)
        {
            onCompleteCallBack.Invoke();
        }
    }

    public void OnCloseTime()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setDelay(1f).setOnComplete(OnComplete).setIgnoreTimeScale(true); ;
    }
    public void OnClose()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(HideMe).setIgnoreTimeScale(true); ;
    }

    public void TopFloat()
    {
        LeanTween.moveLocalY(gameObject, gameObject.transform.localPosition.y + 10f, 0.5f).setOnComplete(BottomFloat).setIgnoreTimeScale(true); ;
    }

    public void BottomFloat()
    {
        LeanTween.moveLocalY(gameObject, gameObject.transform.localPosition.y - 10f, 0.5f).setOnComplete(TopFloat).setIgnoreTimeScale(true); ;
    }

    void HideMe()
    {
        gameObject.SetActive(false);
    }
}
