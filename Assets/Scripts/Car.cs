using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float speed = Random. value;
        transform.Translate(new Vector3(0, 0, 0.05f * speed), Space. Self);
        transform.Rotate(Vector3.up * 0.1f, Space.World);
    }
}
