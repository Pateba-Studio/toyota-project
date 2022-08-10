using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody2D carRigidBody;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;

    private float movement;
    private float carRotate;
    public float speed = 20;
    public float carTorque = 10;
    public float jump = 15;
    public float rotating = 1f;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Vertical");
        carRotate = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            carRigidBody.velocity = Vector2.up * jump;
        }
        
        if(!isGrounded) {
            float rotation = carRotate * rotating;
            transform.Rotate(Vector3.forward * -rotation);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);

        backTire.AddTorque(-movement * speed * Time.deltaTime);
        frontTire.AddTorque(-movement * speed * Time.deltaTime);
        carRigidBody.AddTorque(movement * carTorque * Time.deltaTime);
    }
}