using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 25;
    private int currentHealth;
    public int takeRage= 25;

    public Slider healthSlider;
    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage: " + damage);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerRageMeter PLrm = player.GetComponent<PlayerRageMeter>();
            if (PLrm != null)
            {
                PLrm.TakeRage(takeRage);
            }
            else
            {
                Debug.LogWarning("PlayerRageMeter component not found on Player.");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }

        Destroy(gameObject); // or play death animation
    }

}
