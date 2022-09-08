using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class carController : MonoBehaviour {
    public float carSpeed = 10f;
    public CarManager carManager;
    public Sprite[] spriteMobil;

    float minPos = -1.75f;
    float maxPos = 1.75f;
    float middle;

    Vector3 position;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteMobil[Random.Range(0, spriteMobil.Length)];
        middle = Screen.width / 2;
        position = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        float axis = Input.GetAxis("Horizontal");
        if (axis < -0.1f)
            MoveLeft();
        else if (axis > 0.1f)
            MoveRight();
        else
            SetVelocityZero();   

        position = transform.position;
        position.x = Mathf.Clamp(position.x, minPos, maxPos);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy Car")
            StartCoroutine(CarBlink());
        else if (collision.gameObject.tag == "Wrong Answer")
        {
            if (collision.gameObject.transform.position.x < 0)
            {
                if (gameObject.transform.position.x < 0)
                {
                    StartCoroutine(CarBlink());
                    StartCoroutine(carManager.SpawnPanel(false));
                }
                else StartCoroutine(carManager.SpawnPanel(true));
            }
            else
            {
                if (gameObject.transform.position.x >= 0)
                {
                    StartCoroutine(CarBlink());
                    StartCoroutine(carManager.SpawnPanel(false));
                }
                else StartCoroutine(carManager.SpawnPanel(true));
            }
        }
    }
    
    IEnumerator CarBlink()
    {
        Color tmp = spriteRenderer.color;
        tmp.a = .5f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
        tmp.a = 1f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
        tmp.a = .5f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
        tmp.a = 1f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
        tmp.a = .5f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
        tmp.a = 1f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(.5f);
    }

    public void MoveLeft()
    {
        transform.position += Vector3.left * carSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 30), Time.deltaTime * 3);
    }

    public void MoveRight()
    {
        if (carManager.videoHandler.GetComponent<VideoScript>().videoPlayer.renderMode != VideoRenderMode.CameraNearPlane)
        {
            transform.position += Vector3.right * carSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 330), Time.deltaTime * 3);
        }
    }

    public void SetVelocityZero()
    {
        if (carManager.videoHandler.GetComponent<VideoScript>().videoPlayer.renderMode != VideoRenderMode.CameraNearPlane)
        {
            rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 8);
        }
    }
}
