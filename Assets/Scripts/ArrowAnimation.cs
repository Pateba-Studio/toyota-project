using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/* A script that is attached to the `Arrow` game object. It is responsible for the
animation of the arrow when it is shot. */
public class ArrowAnimation : MonoBehaviour
{
  private Animator _animator;
  public GameObject crosshairGO;
  public GameObject arrowA, arrowB, arrowC, arrowD;
  public GameObject bulleyeA, bulleyeB, bulleyeC, bulleyeD;

  public GameObject panelAnswerTrue, panelAnswerFalse;

  private void Awake()
  {
    _animator = GetComponent<Animator>();

    arrowA.SetActive(false);
    arrowB.SetActive(false);
    arrowC.SetActive(false);
    arrowD.SetActive(false);

    panelAnswerFalse.SetActive(false);
    panelAnswerTrue.SetActive(false);
  }

  //Update to check if the target being hit by crosshair raycast
  private void Update()
  {
    RaycastHit2D hit = Physics2D.Raycast(crosshairGO.transform.position, crosshairGO.transform.TransformDirection(Vector3.forward), 5f);

    if (Input.GetMouseButtonUp(0))
    {
      _animator.SetTrigger("Shooting");

      if (hit.collider == null)
      {
        return;
      }
      else if (hit.collider.name == "A")
      {
        StartCoroutine(ShowArrowShooted(arrowA, bulleyeA));
        StartCoroutine(PanelFalse());
      }
      else if (hit.collider.name == "B")
      {
        StartCoroutine(ShowArrowShooted(arrowB, bulleyeB));
        StartCoroutine(PanelFalse());
      }
      else if (hit.collider.name == "C")
      {
        StartCoroutine(ShowArrowShooted(arrowC, bulleyeC));
        StartCoroutine(PanelTrue());
      }
      else if (hit.collider.name == "D")
      {
        StartCoroutine(ShowArrowShooted(arrowD, bulleyeD));
        StartCoroutine(PanelFalse());
      }
    }
  }

  //set animation for bouncy effect on target
  IEnumerator ShowArrowShooted(GameObject arrowObj, GameObject bullseye)
  {
    bullseye.transform.DOScale(.08f, .3f);
    yield return new WaitForSeconds(.3f);
    bullseye.transform.DOScale(0.12f, .3f).SetEase(Ease.OutBounce);
    yield return new WaitForSeconds(.6f);
    arrowObj.SetActive(true);
  }

  //set spawn panel answers is true
  IEnumerator PanelTrue()
  {
    yield return new WaitForSeconds(.6f);
    panelAnswerTrue.SetActive(true);
    yield return new WaitForSeconds(1f);
    panelAnswerTrue.SetActive(false);
  }

  //set spawn panel answers is false
  IEnumerator PanelFalse()
  {
    yield return new WaitForSeconds(.6f);
    panelAnswerFalse.SetActive(true);
    yield return new WaitForSeconds(1f);
    panelAnswerFalse.SetActive(false);
  }
}
