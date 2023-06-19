using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public Variables:
    public bool _hasPowerUp = false;
    [Header("GameObject:")]
    public GameObject PowerupIndicator;

    //Private Not Serialized Variables:
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
    private readonly float _playerSpeed = 10.0f;
    private readonly float _powerUpStrength = 15.0f;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");
    }

    void Update()
    {
        float verticalInput = _playerSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        _playerRb.AddForce(_focalPoint.transform.forward * verticalInput, ForceMode.Impulse);

        PowerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp")) 
        {
            _hasPowerUp = true;
            PowerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine() 
    {
        yield return new WaitForSeconds(7);
        _hasPowerUp = false;
        PowerupIndicator.gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _hasPowerUp) 
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * _powerUpStrength, ForceMode.Impulse);
            Debug.Log($"Collided with {collision.gameObject.name} with power up set to {_hasPowerUp}.");
        }    
    }
}
