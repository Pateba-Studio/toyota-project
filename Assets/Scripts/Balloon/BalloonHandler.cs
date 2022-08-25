using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonHandler : MonoBehaviour
{
    public bool isSpawned;
    public int answerId;
    public float flySpeed;
    public BaloonManager baloonManager;
    public AnimationClip popOutAnimation;
    public Transform targetMove;

    // Start is called before the first frame update
    void Start()
    {
        baloonManager = GameObject.Find("Baloon Manager").GetComponent<BaloonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned) transform.position = Vector3.MoveTowards(transform.position, targetMove.position, flySpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (baloonManager.isPlay) 
        {
            GetComponent<Animator>().SetTrigger("isPopOut");
            StartCoroutine(DestroyBaloon()); 
        }
    }

    public IEnumerator DestroyBaloon()
    {
        yield return new WaitForSeconds(popOutAnimation.length);

        if (answerId == baloonManager.correctAnswer)
        {
            StartCoroutine(baloonManager.PopUpHandler(true, gameObject));
            Animator[] animators = transform.parent.GetComponentsInChildren<Animator>();
            for (int i = 0; i < animators.Length; i++)
                animators[i].SetTrigger("isPopOut");
        }
        else
            StartCoroutine(baloonManager.PopUpHandler(false, gameObject));
    }
}
