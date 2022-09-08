using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
    float speed = 7f;
    [SerializeField] Sprite[] spriteObs;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteObs[Random.Range(0, spriteObs.Length)];
    }
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
