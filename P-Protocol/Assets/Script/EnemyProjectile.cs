using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Debug.Log("Player hit by projectile");
            Destroy(gameObject);
        }
    }
}
