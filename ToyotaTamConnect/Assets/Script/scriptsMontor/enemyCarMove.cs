using UnityEngine;
using System.Collections;

public class enemyCarMove : MonoBehaviour {
    public static float speed = 5f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0,-0.5f,0) * speed * Time.deltaTime);
	}
}
