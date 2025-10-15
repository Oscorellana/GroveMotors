using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Or TMPro if using TextMeshPro

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    void Update()
    {
        // Detect ? key (Shift + / on most keyboards)
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            // Toggle visibility
            bool isActive = instructionsPanel.activeSelf;
            instructionsPanel.SetActive(!isActive);
        }
    }
}

