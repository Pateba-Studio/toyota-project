using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabObject;
    [SerializeField] float minCoorX = -2.4f;
    [SerializeField] float maxCoorX = 2.4f;
    [SerializeField] float startTime = 0;
    [SerializeField] float spawnRate = 2.5f;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnObject", startTime, spawnRate);
    }
    void SpawnObject()
    {
        Vector3 pos = new Vector3(Random.Range(minCoorX, maxCoorX), transform.position.y, transform.position.z);
        Instantiate(prefabObject, pos, transform.rotation);
    }
}