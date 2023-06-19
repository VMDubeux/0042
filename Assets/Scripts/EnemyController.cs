using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private readonly float _speed = 100.0f;
    private Rigidbody _rbEnemy;
    private GameObject _player;

    void Start()
    {
        _rbEnemy = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized;
        _rbEnemy.AddForce(_speed * Time.deltaTime * lookDirection);

        if (transform.position.y < -10.0f) Destroy(gameObject);
    }
}
