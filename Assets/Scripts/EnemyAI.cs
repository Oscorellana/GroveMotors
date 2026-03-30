using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// NavMesh-driven enemy AI with three states: Roam, Chase, and Attack.
/// Triggers a game-over scene load when the player enters attack range.
/// </summary>
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
    public string gameOverSceneName = "GameOverScene";

    private NavMeshAgent agent;
    private Animator animator;

    private float roamTimer;
    private Vector3 roamDestination;
    private bool hasTriggeredGameOver;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        roamTimer = timeBetweenRoams;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        PickNewRoamDestination();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
            AttackPlayer();
        else if (distanceToPlayer <= detectionRadius)
            ChasePlayer();
        else
            Roam();

        animator.SetFloat(SpeedHash, agent.velocity.magnitude);
    }

    /// <summary>Sets the NavMesh destination to the player's current position.</summary>
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    /// <summary>Wanders to random NavMesh positions within <see cref="roamRadius"/>.</summary>
    private void Roam()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= timeBetweenRoams || Vector3.Distance(transform.position, roamDestination) < 1f)
        {
            PickNewRoamDestination();
            roamTimer = 0f;
        }

        agent.SetDestination(roamDestination);
    }

    private void PickNewRoamDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
            roamDestination = hit.position;
        else
            roamDestination = transform.position;
    }

    /// <summary>Faces the player, stops movement, and loads the game-over scene.</summary>
    private void AttackPlayer()
    {
        if (hasTriggeredGameOver) return;
        hasTriggeredGameOver = true;

        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
        agent.ResetPath();

        SceneManager.LoadScene(gameOverSceneName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}