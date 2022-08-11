using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditAnimateScript : MonoBehaviour
{
    public GameObject car1, car2, car3, car4;
    public Transform spawnPoint;

    public float spawnRate = 4f;
    private float nextSpawn = 0f;
    private int whatToSpawn;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            whatToSpawn = Random.Range(1, 5);

            switch (whatToSpawn)
            {
                case 1:
                    Instantiate(car1, spawnPoint.position, spawnPoint.rotation);
                    break;
                case 2:
                    Instantiate(car2, spawnPoint.position, spawnPoint.rotation);
                    break;
                case 3:
                    Instantiate(car3, spawnPoint.position, spawnPoint.rotation);
                    break;
                case 4:
                    Instantiate(car4, spawnPoint.position, spawnPoint.rotation);
                    break;
            }

            nextSpawn = Time.time + spawnRate;
        }
    }
}
