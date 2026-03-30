using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackTrigger : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private string playerTag = "Player"; // Must match your player's tag
    [SerializeField] private float attackDelay = 0.5f; // short delay before triggering game over
    [SerializeField] private string gameOverSceneName = "GameOver"; // name of your Game Over scene

    private bool hasAttacked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAttacked) return;

        if (other.CompareTag(playerTag))
        {
            hasAttacked = true;
            Debug.Log("Player hit! Game Over...");
            StartCoroutine(TriggerGameOver());
        }
    }

    private System.Collections.IEnumerator TriggerGameOver()
    {
        yield return new WaitForSeconds(attackDelay);

        // Load the Game Over scene
        SceneManager.LoadScene(gameOverSceneName);
    }
}

