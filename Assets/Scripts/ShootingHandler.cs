using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
  private Animator _animator;
  public GameObject crosshairGO;

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      _animator.SetTrigger("Shooting");
    }
  }
}
