using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject car;

    public void Spawn()
    {
        InvokeRepeating("CreateCar", 1f, 1.5f);
    }


    void CreateObstacle()
    {
        Instantiate(car);
    }
}
