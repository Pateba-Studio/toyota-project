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

  public ArcherManager archerManager;
  public GameObject bowController;

  private float timeCount = 3f;
  private float remainingTime = 0f;

  private void Awake()
  {
    _animator = GetComponent<Animator>();
    remainingTime = timeCount;

    arrowA.SetActive(false);
    arrowB.SetActive(false);
    arrowC.SetActive(false);
    arrowD.SetActive(false);
  }

  //Update to check if the target being hit by crosshair raycast
  private void Update()
  {
    RaycastHit2D hit = Physics2D.Raycast(crosshairGO.transform.position, crosshairGO.transform.TransformDirection(Vector3.forward), 5f);

    if (Input.GetMouseButtonUp(0))
    {
      bowController.GetComponent<BowController>().enabled = false;

      if (bowController.GetComponent<BowController>().enabled == false)
      {
        _animator.SetTrigger("Shooting");

        if (hit.collider == null)
        {
          bowController.GetComponent<BowController>().enabled = true;
          return;
        }
        else if (hit.collider.name == "A")
        {
          StartCoroutine(ShowArrowShooted(arrowA, bulleyeA));
          archerManager.ChooseAnswer(0);
        }
        else if (hit.collider.name == "B")
        {
          StartCoroutine(ShowArrowShooted(arrowB, bulleyeB));
          archerManager.ChooseAnswer(1);
        }
        else if (hit.collider.name == "C")
        {
          StartCoroutine(ShowArrowShooted(arrowC, bulleyeC));
          archerManager.ChooseAnswer(2);
        }
        else if (hit.collider.name == "D")
        {
          StartCoroutine(ShowArrowShooted(arrowD, bulleyeD));
          archerManager.ChooseAnswer(3);
        }

        bowController.GetComponent<BowController>().enabled = true;
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

    yield return new WaitForSeconds(1f);
    arrowObj.SetActive(false);
  }
}
