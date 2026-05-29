using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public Light redLight;
    public Light blueLight;
    public AudioSource sirenAudio;
    
    private NavMeshAgent agent;
    private Animator anim;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        if (anim != null)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
            anim.Play("PoliceCar_Siren", 0, 0);
            anim.speed = 1f;
        }
        
        if (sirenAudio != null)
        {
            sirenAudio.loop = true;
            sirenAudio.Play();
        }
    }
    
    void Update()
    {
        // Находим ближайшего игрока
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;
        
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = player.transform;
            }
        }
        
        if (closestPlayer != null && agent != null)
        {
            agent.SetDestination(closestPlayer.position);
        }
        
        // Звук от ближайшего игрока
        if (sirenAudio != null && closestPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, closestPlayer.position);
            float maxDistance = 30f;
            float minDistance = 3f;
            float volume = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            sirenAudio.volume = volume;
        }
    }
}