using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float turnSpeed = 100f;
    public float acceleration = 5f;
    public float gravityForce = 20f;

    private float currentSpeed = 0f;
    private float currentTurn = 0f;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.mass = 1f;
        rb.drag = 2f;
        rb.angularDrag = 2f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float turn = 0f;

        if (Input.GetKey(KeyCode.Q)) turn = -1f;
        else if (Input.GetKey(KeyCode.E)) turn = 1f;

        currentSpeed = Mathf.Lerp(currentSpeed, vertical * moveSpeed, Time.deltaTime * acceleration);
        currentTurn = Mathf.Lerp(currentTurn, turn * turnSpeed, Time.deltaTime * acceleration);
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * currentSpeed;
        moveDirection.y = 0;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        rb.angularVelocity = new Vector3(0, currentTurn * Mathf.Deg2Rad, 0);

        rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}
