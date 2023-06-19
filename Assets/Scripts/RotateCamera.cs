using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private readonly float _speedRotation = 50.0f;

    void Update()
    {
        float horizontalInput = _speedRotation * Time.deltaTime * Input.GetAxis("Horizontal") ;

        transform.Rotate(Vector3.up.normalized, angle: horizontalInput);
    }
}
