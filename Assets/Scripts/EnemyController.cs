using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Public Variables:
    public bool _isBoss = false;
    
    //Internal Variables:
    internal int _miniEnemySpawnCount = 0;

    //Private Variables:
    private float _spawnInterval = 3;
    private float _nextSpawn;
    private float _speed = 100.0f;
    private SpawnManager _spawnManager;
    private Rigidbody _rbEnemy;
    private GameObject _player;

    void Start()
    {
        if (gameObject.CompareTag("EnemyM")) _speed = 200.0f;
        else if (gameObject.CompareTag("EnemyH")) _speed = 300.0f;
        else if (gameObject.CompareTag("Boss")) _speed = 350.0f;
        _rbEnemy = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_isBoss) _spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized;
        _rbEnemy.AddForce(_speed * Time.deltaTime * lookDirection);

        if (transform.position.y < -10.0f) Destroy(gameObject);

        if (_isBoss) 
        {
            if (Time.time > _nextSpawn) 
            {
                _nextSpawn = Time.time + _spawnInterval;
                _spawnManager.SpawnMiniEnemy(_miniEnemySpawnCount);
            }
        }
    }
}
