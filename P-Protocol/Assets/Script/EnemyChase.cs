using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChaseNavmesh2_5D : MonoBehaviour
{
    public int damage = 20;
    private Transform player;
    private NavMeshAgent agent;
    private float fixedZ; // The Z position to lock to (2.5D effect)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        // Lock Z movement
        fixedZ = transform.position.z;

        // Optional: prevent rotation and Y axis movement smoothing
        agent.updateUpAxis = true;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (player == null) return;

        // Follow player in X/Y only, lock Z
        Vector3 target = player.position;
        target.z = fixedZ; // Force Z lock for 2.5D effect
        agent.SetDestination(target);
    }

    private void LateUpdate()
    {
        // Lock Z axis to ensure it doesn't drift
        Vector3 pos = transform.position;
        pos.z = fixedZ;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
