using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chicken : MonoBehaviour, IDamageable
{
    public float maxHP = 10f;
    public float wanderRadius = 6f;
    public float changeInterval = 2.5f;
    public float fleeDistance = 6f;
    public float fleeSpeed = 5f;
    float hp;
    NavMeshAgent agent;
    Transform player;
    float timer = 0f;

    void Start()
    {
        hp = maxHP;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (player != null)
        {
            float d = Vector3.Distance(transform.position, player.position);
            if (d <= fleeDistance)
            {
                // 逃げる
                Vector3 dir = (transform.position - player.position).normalized;
                Vector3 fleeTarget = transform.position + dir * wanderRadius;
                agent.speed = fleeSpeed;
                agent.SetDestination(fleeTarget);
                return;
            }
        }

        if (timer <= 0f)
        {
            timer = changeInterval;
            Vector3 rand = transform.position + Random.insideUnitSphere * wanderRadius;
            rand.y = transform.position.y;
            agent.speed = 1.8f;
            agent.SetDestination(rand);
        }
    }

    public void TakeDamage(float amount, GameObject source = null)
    {
        hp -= amount;
        if (hp <= 0f) Destroy(gameObject);
    }
}
