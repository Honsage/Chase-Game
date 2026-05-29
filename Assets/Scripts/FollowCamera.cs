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

    private float currentDistance;

    void Start()
    {
        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        Vector3 desiredPosition = target.position - target.forward * currentDistance + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}