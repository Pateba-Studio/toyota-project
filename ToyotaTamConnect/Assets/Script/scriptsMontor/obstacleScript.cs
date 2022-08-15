using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
    float speed = 14f;
    //[SerializeField] Sprite[] spriteObs;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    gameObject.GetComponent<SpriteRenderer>().sprite = spriteObs[Random.Range(0, spriteObs.Length)];
    //}
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -0.5f, 0) * speed * Time.deltaTime);
    }
}
