using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("GameObject 1:")]
    public GameObject Enemy; //Add GameObject in this Slot
    [Header("GameObject 2:")]
    public GameObject PowerUps;
    private readonly float RandomPos = 9.0f;
    private int _enemyCount;
    private int _waveNumber = 1;

    void Start()
    {
        SpawnEnemyWave(_waveNumber);
    }

    void Update()
    {
        _enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (_enemyCount == 0) 
        {
            _waveNumber++;
            SpawnEnemyWave(_waveNumber);
            Instantiate(PowerUps, GenerateSpawnPos(), PowerUps.transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(Enemy, GenerateSpawnPos(), Enemy.transform.rotation); //Instantiate Method
        }
    }

    Vector3 GenerateSpawnPos()
    {
        float xPos = Random.Range(-RandomPos, RandomPos); //Generates Random xPos Numbers
        float zPos = Random.Range(-RandomPos, RandomPos); //Generates Random zPos Numbers
        Vector3 SpawnPos = new(xPos, 0.25f, zPos); //Spawn Position
        return SpawnPos;
    }
}
