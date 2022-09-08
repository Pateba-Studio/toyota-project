using UnityEngine;
using System.Collections;

public class SpawnedObjectScript : MonoBehaviour {
    [SerializeField] float speed = 5f;
    [SerializeField] Sprite[] objectSprites;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().sprite = objectSprites[Random.Range(0, objectSprites.Length)];
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
	}
}
