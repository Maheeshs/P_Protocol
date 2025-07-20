using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage=25;
    


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Enemy"))
        {
            Debug.Log(damage);
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if(other.CompareTag("Npc"))
        {
            NPC npc= other.GetComponent<NPC>();
            npc.takePlRage(30);
            Destroy(gameObject);
        }
    }
    
}
