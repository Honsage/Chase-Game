using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 5f;

    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;

    public bool rotateWithCar = true;

    private float currentDistance;

    void Start()
    {
        if (target == null)
        {
            GameObject car = GameObject.FindGameObjectWithTag("Player");
            if (car != null)
                target = car.transform;
        }
        
        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (target == null)
            return;
            
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        
        Vector3 desiredPosition;
        
        if (rotateWithCar)
        {
            desiredPosition = target.position - target.forward * currentDistance + Vector3.up * height;
        }
        else
        {
            desiredPosition = target.position - Vector3.forward * currentDistance + Vector3.up * height;
        }
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.LookAt(target.position + Vector3.up * 1f);
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            
            Gizmos.color = Color.blue;
            Vector3 closePos = target.position - target.forward * minDistance + Vector3.up * height;
            Vector3 farPos = target.position - target.forward * maxDistance + Vector3.up * height;
            Gizmos.DrawWireSphere(closePos, 0.3f);
            Gizmos.DrawWireSphere(farPos, 0.3f);
            Gizmos.DrawLine(closePos, farPos);
        }
    }
    
}
