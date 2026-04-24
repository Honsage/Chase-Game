using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public Transform player;
    public Light redLight;
    public Light blueLight;
    
    private NavMeshAgent agent;
    private Animator anim;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }


        if (anim != null)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
            anim.Play("PoliceCar_Siren", 0, 0);
            anim.speed = 1f;
            Debug.Log("Animator started playing: " + anim.GetCurrentAnimatorStateInfo(0).length);

        }
    }
    
    void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);
        }

        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Normalized time: " + stateInfo.normalizedTime);
        }
    }

    
}