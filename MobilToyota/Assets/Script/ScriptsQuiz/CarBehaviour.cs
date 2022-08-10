using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public int MAX_SPEED, MIN_SPEED, MAX_DIST,FIRST_COLOR;
    Vector3 originalPos;
    [SerializeField] public Sprite[] carColor;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        int speed = Random.Range(MAX_SPEED  , MIN_SPEED);
        originalPos = gameObject.transform.position;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed,0);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("End"))
        {
            StartCoroutine("RandomTheCar");
            gameObject.transform.position = originalPos;
            Start();
        }
    }
    private IEnumerator RandomTheCar()
    {
        coroutineAllowed = false;
        int randomColorPicker = Random.Range(0,4);
        rend.sprite = carColor[randomColorPicker];
        yield return new WaitForSeconds(0.0001f);
   

        coroutineAllowed = true;
    }

}
