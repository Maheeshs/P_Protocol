using UnityEngine;
using System.Collections.Generic;

public class EnemyActivator : MonoBehaviour
{

    public GameObject[] Enemy; // Assign in Inspector
    public Transform[] spawnPoints;
    private bool[] spawned;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        spawned = new bool[spawnPoints.Length];
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null) return;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawned[i]) continue;
                Instantiate(Enemy[i], spawnPoints[i].position, spawnPoints[i].rotation);
                spawned[i] = true;
            }
        }
    }
}
