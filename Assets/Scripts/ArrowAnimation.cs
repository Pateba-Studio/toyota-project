using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
  private Animator _animator;
  public GameObject crosshairGO;
  public GameObject arrowA, arrowB, arrowC, arrowD;

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
        StartCoroutine(ShowArrowShooted(arrowA));
        StartCoroutine(PanelFalse());

      }
      else if (hit.collider.name == "B")
      {
        StartCoroutine(ShowArrowShooted(arrowB));
        StartCoroutine(PanelFalse());
      }
      else if (hit.collider.name == "C")
      {
        StartCoroutine(ShowArrowShooted(arrowC));
        StartCoroutine(PanelTrue());
      }
      else if (hit.collider.name == "D")
      {
        StartCoroutine(ShowArrowShooted(arrowD));
        StartCoroutine(PanelFalse());
      }
    }
  }

  IEnumerator ShowArrowShooted(GameObject arrowObj)
  {
    yield return new WaitForSeconds(.6f);
    arrowObj.SetActive(true);
  }

  IEnumerator PanelTrue()
  {
    yield return new WaitForSeconds(.6f);
    panelAnswerTrue.SetActive(true);
    yield return new WaitForSeconds(1f);
    panelAnswerTrue.SetActive(false);
  }

  IEnumerator PanelFalse()
  {
    yield return new WaitForSeconds(.6f);
    panelAnswerFalse.SetActive(true);
    yield return new WaitForSeconds(1f);
    panelAnswerFalse.SetActive(false);
  }
}
