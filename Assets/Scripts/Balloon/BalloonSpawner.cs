using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public bool isDone;
    public float spawnCooldown;
    public BaloonManager baloonManager;
    public List<GameObject> balloonObject;
    public List<BalloonHandler> balloons;

    // Start is called before the first frame update
    void Start()
    {
        baloonManager = GameObject.Find("Baloon Manager").GetComponent<BaloonManager>();
        StartCoroutine(SpawnBalloon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnBalloon()
    {
        isDone = false;
        if (balloons.Count == 0) { isDone = true; yield break; }

        int randomBalloon = Random.Range(0, balloons.Count);
        balloons[randomBalloon].isSpawned = true;
        balloons.RemoveAt(randomBalloon);

        yield return new WaitForSeconds(spawnCooldown);
        StartCoroutine(SpawnBalloon());
    }
}
