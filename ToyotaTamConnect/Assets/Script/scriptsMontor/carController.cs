using UnityEngine;
using System.Collections;

public class carController : MonoBehaviour {
    public float carSpeed = 10f;
    private float minPos = -2.8f;
    private float maxPos = 2.8f;

    Vector3 position;
    [SerializeField] Sprite[] spriteMobil;
    public uiManager ui;

    Rigidbody2D rb;
    float middle;

    bool currentPlatformAndroid = false;

    [SerializeField] SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //#if (UNITY_ANDROID)
        //{
        //    currentPlatformAndroid = true;
        //}
        //#else
        //{
        //    currentPlatformAndroid = false;
        //}
        //#endif
    }

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteMobil[Random.Range(0, spriteMobil.Length)];
        middle = Screen.width / 2;
        position = transform.position;
        //Debug.Log("Is Android : " + currentPlatformAndroid);
	}

    // Update is called once per frame
    void Update()
    {
        if (currentPlatformAndroid)
        {
            if(ControlManager.isTilt)
            {
                AccelerometerMove();
            }
            else
            {
                TouchMove();   
            }
        }
        else
        {
            float axis = Input.GetAxis("Horizontal");
            if (axis < -0.1f)
            {
                MoveLeft();
            }
            else if (axis > 0.1f)
            {
                MoveRight();
            }
            else
                SetVelocityZero();
        }

        position = transform.position;
        position.x = Mathf.Clamp(position.x, minPos, maxPos);
        transform.position = position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy Car")
        {
            //ui.GameOverActivated();
            StartCoroutine(CarBlink());
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
    void TouchMove()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.position.x < middle && touch.phase == TouchPhase.Began)
            {
                MoveLeft();
            }
            else if(touch.position.x > middle && touch.phase == TouchPhase.Began)
            {
                MoveRight();
            }
        }
        else
        {
            SetVelocityZero();
        }
    }
    void AccelerometerMove()
    {
        float x = Input.acceleration.x;

        //Debug.Log(x);

        if(x < -0.1f)
        {
            MoveLeft();
        }
        else if(x > 0.1f)
        {
            MoveRight();
        }
        else {
            SetVelocityZero();
        }
    }

    public void MoveLeft()
    {
        transform.position += Vector3.left * carSpeed * Time.deltaTime;
        //rb.velocity = new Vector2(-carSpeed, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 30), Time.deltaTime * 3);
    }

    public void MoveRight()
    {
        transform.position += Vector3.right * carSpeed * Time.deltaTime;
        //rb.velocity = new Vector2(carSpeed, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 330), Time.deltaTime*3);
    }

    public void SetVelocityZero()
    {
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 8);
    }
}
