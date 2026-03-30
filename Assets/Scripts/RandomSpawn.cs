using UnityEngine;

/// <summary>
/// Randomly places this GameObject at one of the assigned spawn points on Start.
/// </summary>
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

        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.SetPositionAndRotation(randomSpawn.position, randomSpawn.rotation);
    }
}
