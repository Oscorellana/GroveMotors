using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // for Game Over

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRadius = 10f;
    public float roamRadius = 15f;
    public float attackRange = 2f;
    public float timeBetweenRoams = 5f;

    [Header("References")]
    public Transform player;
    public string gameOverSceneName = "GameOverScene"; // change if your Game Over scene has a different name

    private NavMeshAgent agent;
    private Animator animator;

    private float roamTimer;
    private Vector3 roamDestination;
    private bool playerInRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        roamTimer = timeBetweenRoams;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        PickNewRoamDestination();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // === Behavior switching ===
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }

        // === Animation Control ===
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);
    }

    // ========== AI Behaviors ==========
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void Roam()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= timeBetweenRoams || Vector3.Distance(transform.position, roamDestination) < 1f)
        {
            PickNewRoamDestination();
            roamTimer = 0f;
        }

        agent.SetDestination(roamDestination);
    }

    void PickNewRoamDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius + transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamDestination = hit.position;
        }
        else
        {
            roamDestination = transform.position;
        }
    }

    void AttackPlayer()
    {
        // Look at player
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        agent.ResetPath(); // stop moving

        // Optional: trigger an attack animation later if you add one
        // animator.SetTrigger("Attack");

        Debug.Log("Player hit! Game Over!");
        SceneManager.LoadScene(gameOverSceneName);
    }

    // ========== Attack Trigger Collider ==========
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered attack zone!");
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visual debug
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}


