using UnityEngine;

public class NPC : MonoBehaviour
{
    public float roamRadius = 10f;
    public float roamTimer = 5f;
    public Animator animator;

    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        timer = roamTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= roamTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, roamRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out navHit, distance, UnityEngine.AI.NavMesh.AllAreas);

        return navHit.position;
    }
}
