using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundMask, playerMask;

    [Header("Patrol Settings")]
    public float walkPointRange = 10f;
    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Detection Settings")]
    public float sightRange = 10f;
    private bool playerInSightRange;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if player is within detection range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (!playerInSightRange)
            Patrol();
        else
            ChasePlayer();
    }

    private void Patrol()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Pick a random point on the ground within range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(potentialPoint, -transform.up, 2f, groundMask))
        {
            walkPoint = potentialPoint;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
            agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

