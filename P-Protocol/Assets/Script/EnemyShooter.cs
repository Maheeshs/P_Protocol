using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour
{
    public float detectionRange = 30f;
    public float shootingRange = 10f;
    public float fireRate = 1f;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float shootForce = 15f;

    private Transform player;
    private NavMeshAgent agent;
    private float fireCooldown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Rotate towards player
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.forward = lookDir;

        if (distance <= shootingRange)
        {
            agent.isStopped = true;
            ShootAtPlayer();
        }
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void ShootAtPlayer()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = firePoint.forward * shootForce;

            fireCooldown = 1f / fireRate;
        }
    }
}
