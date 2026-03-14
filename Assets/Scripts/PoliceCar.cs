using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public Transform player;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }
    
    void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }
}