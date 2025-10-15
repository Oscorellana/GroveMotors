using UnityEngine;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject hintPanel;

    void Start()
    {
        // Make sure starting state is correct
        instructionsPanel.SetActive(false);
        hintPanel.SetActive(true);
    }

    void Update()
    {
        // Detect ? key (Shift + /)
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            bool showingInstructions = !instructionsPanel.activeSelf;

            // Toggle panels
            instructionsPanel.SetActive(showingInstructions);
            hintPanel.SetActive(!showingInstructions);
        }
    }
}
