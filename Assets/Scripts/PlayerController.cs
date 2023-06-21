using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public Variables:
    [Header("Script PowerUp (Type):")]
    public PowerUpsType CurrentPowerUps = PowerUpsType.None;
    [Header("Complementar GameObject 1:")]
    public GameObject BulletPrefab;
    [Header("Complementar GameObject 2:")]
    public GameObject PowerupIndicator;

    //Private Not Serialized Variables:
    private GameObject _tmpBullet;
    private Coroutine _powerupCountdown;
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
    private bool _smashing = false;
    private float _floorY;

    //Private Not Serialized && Readonly Variables:
    private readonly float _playerSpeed = 10.0f;
    private readonly float _powerUpStrength = 20.0f;
    private readonly float _hangTime = 0.5f;
    private readonly float _smashSpeed = 10.0f;
    private readonly float _explosionForce = 10.0f;
    private readonly float _explosionRadius = 15.0f;

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

        if (CurrentPowerUps == PowerUpsType.Bullets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchBullets();
        }

        if (CurrentPowerUps == PowerUpsType.Smash && Input.GetKeyDown(KeyCode.Space) && !_smashing)
        {
            _smashing = true;
            StartCoroutine(Smash());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            CurrentPowerUps = other.gameObject.GetComponent<PowerUps>().powerUpsType;
            PowerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);

            if (_powerupCountdown != null)
            {
                StopCoroutine(_powerupCountdown);
            }
            _powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        _floorY = transform.position.y; //Store the y position before taking off
        float jumpTime = Time.time + _hangTime; //Calculate the amount of time we will go up
        while (Time.time < jumpTime) //move the player up while still keeping their x velocity.
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, _smashSpeed);
            yield return null;
        }
        while (transform.position.y > _floorY) //Now move the player down
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, -_smashSpeed * 2);
            yield return null;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(_explosionForce,
                    transform.position, _explosionRadius, 1.0f, ForceMode.Impulse);
            }
        }
        _smashing = false;
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        CurrentPowerUps = PowerUpsType.None;
        PowerupIndicator.gameObject.SetActive(false);
    }



    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("EnemyM") ||
            collision.gameObject.CompareTag("EnemyH")) &&
            CurrentPowerUps == PowerUpsType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * _powerUpStrength, ForceMode.Impulse);
            Debug.Log($"Collided with {collision.gameObject.name} with power up set to {CurrentPowerUps}.");
        }
    }

    void LaunchBullets()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            _tmpBullet = Instantiate(BulletPrefab, transform.position + Vector3.up, Quaternion.identity);
            _tmpBullet.GetComponent<BulletsBehaviour>().Fire(enemy.transform);
        }
    }
}
