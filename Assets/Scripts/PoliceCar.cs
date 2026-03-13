using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 5f;
    public float updateRate = 0.5f;
    public float moveForce = 1000f;
    public float turnSpeed = 5f;
    
    private NavMeshAgent agent;
    private Rigidbody rb;
    private float timer = 0f;
    private Vector3 targetDirection;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        agent.speed = 20f;
        agent.angularSpeed = 120f;
        agent.acceleration = 15f;
        agent.stoppingDistance = stopDistance;
        agent.radius = 1f;
        agent.height = 1f;
        
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.drag = 2f;
        rb.angularDrag = 1f;
    }
    
    void Update()
    {
        if (player == null || agent == null) return;
        
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            agent.SetDestination(player.position);
        }
        
        if (agent.hasPath && agent.path.corners.Length > 1)
        {
            Vector3 nextPoint = agent.path.corners[1];
            targetDirection = (nextPoint - transform.position).normalized;
            targetDirection.y = 0;
        }
    }
    
    void FixedUpdate()
    {
        if (agent == null || !agent.hasPath) return;
        
        if (targetDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                rb.AddForce(transform.forward * moveForce * Time.fixedDeltaTime, ForceMode.Force);
            }
        }
        
        if (rb.velocity.magnitude > agent.speed)
        {
            rb.velocity = rb.velocity.normalized * agent.speed;
        }
    }
    
    void OnDrawGizmos()
    {
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.red;
            Vector3[] path = agent.path.corners;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
                Gizmos.DrawSphere(path[i], 0.2f);
            }
        }
    }
}