using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform levelPart_Start;
    [SerializeField]
    private Transform levelPart_1;
    [SerializeField]
    private CarController player;

    private Vector3 lastEndPosition;

    private void Awake()
    {
        lastEndPosition = levelPart_Start.Find("EndPosition").position;

        int startingLevel = 3;
        for(int i = 0; i < startingLevel; i++)
        {
            SpawnLevelPart();
        }
    }

    private void Update()
    {
    }

    private void SpawnLevelPart()
    {
        Transform lastLevelPartTransform = SpawnLevelPart(lastEndPosition);
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
    }

    private Transform SpawnLevelPart(Vector3 spawnPos)
    {
        Transform levelPartTransform = Instantiate(levelPart_1, spawnPos, Quaternion.identity);
        return levelPartTransform;
    }
}
