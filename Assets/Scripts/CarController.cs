using Unity.Netcode;
using UnityEngine;

public class CarController : NetworkBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 20f;
    public float turnSpeed = 120f;
    public float acceleration = 6f;

    private float currentSpeed = 0f;
    private float currentTurn = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 1f;
        rb.drag = 2f;
        rb.angularDrag = 3f;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (!IsOwner || !enabled) return;

        float vertical = Input.GetAxis("Vertical");
        float turn = 0f;

        if (Input.GetKey(KeyCode.Q)) turn = -1f;
        else if (Input.GetKey(KeyCode.E)) turn = 1f;

        float accelCoef = Input.GetKey(KeyCode.LeftShift) ? 1.75f : 1f;

        currentSpeed = Mathf.Lerp(currentSpeed, vertical * moveSpeed * accelCoef, Time.deltaTime * acceleration);
        currentTurn = Mathf.Lerp(currentTurn, turn * turnSpeed, Time.deltaTime * acceleration);
    }

    void FixedUpdate()
    {
        if (!IsOwner || !enabled) return;

        Vector3 moveDir = transform.forward * currentSpeed;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        rb.angularVelocity = new Vector3(0, currentTurn * Mathf.Deg2Rad, 0);
    }
}