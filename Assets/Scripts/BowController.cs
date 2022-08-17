using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
  public Transform player;
  public float speed = 5.0f;
  private bool touchStart = false;
  private Vector2 pointA;
  private Vector2 pointB;

  public Transform circle;
  public Transform outerCircle;

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
    }
    if (Input.GetMouseButton(0))
    {
      touchStart = true;
      pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
    }
    else
    {
      touchStart = false;
    }
  }

  private void FixedUpdate()
  {
    if (touchStart)
    {
      Vector2 offset = pointB - pointA;
      Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
      moveCharacter(direction * 1);

      circle.transform.position = new Vector2(0 + direction.x, -4.1f + direction.y) * 1;
    }
    else
    {
      circle.transform.position = new Vector2(0, -4.1f) * 1;
    }
  }

  void moveCharacter(Vector2 direction)
  {
    player.Translate(direction * speed * Time.deltaTime);
  }
}