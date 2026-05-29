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
        Transform closestPlayer = GetClosestPlayer();

        if (closestPlayer != null && agent != null)
        {
            agent.SetDestination(closestPlayer.position);
        }

        // Динамическая громкость сирены
        if (sirenAudio != null && closestPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, closestPlayer.position);
            float maxDistance = 30f;
            float minDistance = 3f;
            float volume = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            sirenAudio.volume = volume;
        }
    }

    private Transform GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = p.transform;
            }
        }
        return closest;
    }
}