using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviour : MonoBehaviour
{
    //Private Not Serialized Variables:
    private Transform _target;
    private bool _homing;

    //Private Not Serialized && Readonly Variables:
    private readonly float _bulletStrength = 15.0f;
    private readonly float _aliveTimer = 5.0f;
    private readonly float _speed = 15.0f;

    void Update()
    {
        if (_homing && _target != null)
        {
            Vector3 moveDiretion = (_target.transform.position - transform.position).normalized;
            transform.position += _speed * Time.deltaTime * moveDiretion;
            transform.LookAt(_target);
        }   
    }

    public void Fire(Transform newTarget)
    {
        _target = newTarget;
        _homing = true;
        Destroy(gameObject, _aliveTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_target != null) 
        {
            if (collision.gameObject.CompareTag(_target.tag)) 
            {
                Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRigidbody.AddForce(away * _bulletStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }    
    }
}