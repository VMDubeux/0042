using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("GameObject 1:")]
    public GameObject[] Enemy; //Add GameObject in this Slot
    
    [Header("GameObject 2:")]
    public GameObject[] PowerUpsPrefabs;

    [Header("GameObject 3:")]
    public GameObject BossPrefab;

    [Header("GameObject 4:")]
    public GameObject[] MiniEnemiesPrefabs;

    //Private Variables:
    private readonly float RandomPos = 9.0f;
    private int _enemyCount;
    private int _bossRound = 2;
    private int _waveNumber = 1;

    void Start()
    {
        int randomPowerUps = Random.Range(0, PowerUpsPrefabs.Length);
        Instantiate(PowerUpsPrefabs[randomPowerUps], GenerateSpawnPos(), PowerUpsPrefabs[randomPowerUps].transform.rotation);
        SpawnEnemyWave(_waveNumber);
    }

    void Update()
    {
        _enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (_enemyCount == 0)
        {
            _waveNumber++;
            if (_waveNumber % _bossRound == 0) SpawnBossWave(_waveNumber);
            else SpawnEnemyWave(_waveNumber);
            int randomPowerUps = Random.Range(0, PowerUpsPrefabs.Length);
            Instantiate(PowerUpsPrefabs[randomPowerUps], GenerateSpawnPos(), PowerUpsPrefabs[randomPowerUps].transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, 3);
            Instantiate(Enemy[enemyIndex], GenerateSpawnPos(), Enemy[enemyIndex].transform.rotation); //Instantiate Method
        }
    }

    void SpawnBossWave(int currentRound) 
    {
        int miniEnemysToSpawn;
        if (_bossRound != 0) miniEnemysToSpawn = currentRound / _bossRound;
        else miniEnemysToSpawn = 1;
        var boss = Instantiate(BossPrefab, GenerateSpawnPos(), BossPrefab.transform.rotation);
        boss.GetComponent<EnemyController>()._miniEnemySpawnCount = miniEnemysToSpawn;
    }

    public void SpawnMiniEnemy(int amount) 
    {
        for (int i = 0; i < amount; i++) 
        {
            int randomMini = Random.Range(0, MiniEnemiesPrefabs.Length);
            Instantiate(MiniEnemiesPrefabs[randomMini], GenerateSpawnPos(), MiniEnemiesPrefabs[randomMini].transform.rotation);
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
