using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCar : MonoBehaviour
{
    [SerializeField]
    private Transform checkPoint;
    [SerializeField]
    private Text distanceText;
    [SerializeField] GameObject SoalCok;
    [SerializeField] PauseMenu pauseMenu;

    public SpriteRenderer spriteRenderer;
    public Sprite[] newSprite;
    public string[] informasi;

    public Text text;

    private float _distance;
    private bool finished = false;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        _distance = (checkPoint.transform.position.x - transform.position.x);

        distanceText.text = "Jarak : " + _distance.ToString("F0") + " meter";

        if (_distance <= 0)
        {
            distanceText.text = "Finish!";
            if(!finished)
            {
                SoalCok.SetActive(true);
                finished = true;
            }
        }
        if (transform.position.y <= -100)
        {
            pauseMenu.Restart();
        }
    }

    IEnumerator StopAwhile() {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    void ChangeSprite()
    {
        int num_sprite = Random.Range(0, 11);
        spriteRenderer.sprite = newSprite[num_sprite];
    }
}