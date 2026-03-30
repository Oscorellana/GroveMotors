using UnityEngine;

/// <summary>
/// Toggles the in-game instructions panel on and off with the ? key (Slash).
/// </summary>
public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject hintPanel;

    void Start()
    {
        instructionsPanel.SetActive(false);
        hintPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            bool showInstructions = !instructionsPanel.activeSelf;
            instructionsPanel.SetActive(showInstructions);
            hintPanel.SetActive(!showInstructions);
        }
    }
}
