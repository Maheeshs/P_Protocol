using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPC : MonoBehaviour
{
    public int takeRage = 50;

    
    private void OnTriggerEnter(Collider other)
    {
        

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (other.CompareTag("Sword") )
        {
            // Take rage when hit
            takePlRage(30);
        }
    }


    public void takePlRage(int rage)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerRageMeter plRM = player.GetComponent<PlayerRageMeter>();
        if (plRM != null)
        {
            plRM.TakeRage(rage);
        }
    }
    
}
