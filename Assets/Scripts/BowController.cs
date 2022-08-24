using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A script that controls the movement of the player.
This script attached on the joystick game object*/
public class BowController : MonoBehaviour
{
  public Transform player; //get crosshair object pos data
  public float speed = 5.0f; //speed handle movement crosshair and bow
  private bool touchStart = false;
  private Vector2 pointA; //point that directed into the holding mouse input/touch input pos
  private Vector2 pointB; //point that directed into the mouse input/touch input pos

  public Transform circle; //get joystick game object pos data
  public Transform outerCircle; //get outer circle or buffer joystick object pos data

  public SpriteRenderer spriteRenderer; //get sprite joystick data
  public Sprite newSprite, oldSprite; //get new sprite initiated data attached to the joystick game object sprite

  void Update()
  {
    //check the data input holding position
    if (Input.GetMouseButtonDown(0))
    {
      pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
    }
    //check the data first generated input 
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
    //moving the joystick and crosshair data
    if (touchStart)
    {
      Vector2 offset = pointB - pointA;
      Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
      moveCharacter(direction * 1);

      spriteRenderer.sprite = newSprite;

      circle.transform.position = new Vector2(0 + direction.x, -4.1f + direction.y) * 1;
    }
    //set back the joystick to 0 pos
    else
    {
      circle.transform.position = new Vector2(0, -4.1f) * 1;
      spriteRenderer.sprite = oldSprite;
    }
  }

  //moving the crosshair and bow game object
  void moveCharacter(Vector2 direction)
  {
    player.Translate(direction * speed * Time.deltaTime);
  }
}