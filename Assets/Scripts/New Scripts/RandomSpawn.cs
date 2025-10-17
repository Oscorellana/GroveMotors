using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Possible Spawn Points")]
    [Tooltip("Assign 3 or more spawn points unique to this item.")]
    public Transform[] spawnPoints;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} has no spawn points assigned!", this);
            return;
        }

        // Pick one of the assigned spawn points randomly
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Move this object to that position and rotation
        transform.position = randomSpawn.position;
        transform.rotation = randomSpawn.rotation;
    }
}

