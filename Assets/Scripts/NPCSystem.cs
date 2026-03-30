using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Proximity-triggered typewriter dialogue system.
/// The player presses F inside the trigger collider to open or close the dialogue panel.
/// </summary>
public class NPCSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue")]
    public string[] dialogue;
    public float wordSpeed = 0.05f;

    private int index;
    private bool playerIsClose;

    void Update()
    {
        if (!playerIsClose) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (dialoguePanel.activeInHierarchy)
                ResetDialogue();
            else
                OpenDialogue();
        }
    }

    /// <summary>Opens the dialogue panel and starts typing the first line.</summary>
    public void OpenDialogue()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    /// <summary>Advances to the next dialogue line, or closes the panel at the end.</summary>
    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            ResetDialogue();
        }
    }

    /// <summary>Clears text, resets the index, and hides the dialogue panel.</summary>
    public void ResetDialogue()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    private IEnumerator TypeLine()
    {
        foreach (char letter in dialogue[index])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerIsClose = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ResetDialogue();
        }
    }
}
