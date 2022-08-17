using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
  private Animator _animator;
  public GameObject crosshairGO;
  public GameObject arrowA, arrowB, arrowC, arrowD;

  private void Awake()
  {
    _animator = GetComponent<Animator>();
    arrowA.SetActive(false);
    arrowB.SetActive(false);
    arrowC.SetActive(false);
    arrowD.SetActive(false);
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
      }
      else if (hit.collider.name == "B")
      {
        StartCoroutine(ShowArrowShooted(arrowB));
      }
      else if (hit.collider.name == "C")
      {
        StartCoroutine(ShowArrowShooted(arrowC));
      }
      else if (hit.collider.name == "D")
      {
        StartCoroutine(ShowArrowShooted(arrowD));
      }
    }
  }

  IEnumerator ShowArrowShooted(GameObject arrowObj)
  {
    yield return new WaitForSeconds(.6f);
    arrowObj.SetActive(true);
  }
}
