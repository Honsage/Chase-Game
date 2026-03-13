using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 10.0f, speed = 10.0f, zoomSpeed = 10.0f;
    
    private float _acceleration = 1f;

    private void Update()
    {
        float hor = Input. GetAxis("Horizontal");
        float ver = Input. GetAxis("Vertical");

        float rotateDirection = 0f;

        if (Input. GetKey(KeyCode.Q)) rotateDirection = -1f;
        else if (Input.GetKey(KeyCode.E)) rotateDirection = 1f;

        _acceleration = Input.GetKey(KeyCode.LeftShift) ? 3f : 1f;

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * rotateDirection * _acceleration, Space.World);
        transform.Translate(new Vector3(hor, 0, ver) * Time.deltaTime * speed * _acceleration, Space. Self);

        transform.position += transform.up * zoomSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -19f, 5f),
            transform.position.z
        );
    }
}
