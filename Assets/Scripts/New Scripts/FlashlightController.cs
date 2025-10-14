using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;   // Assign the spotlight here
    public KeyCode toggleKey = KeyCode.F;  // Key to toggle flashlight

    private bool isOn = true;

    void Start()
    {
        if (flashlightLight == null)
        {
            flashlightLight = GetComponentInChildren<Light>();
        }

        // Start with flashlight ON
        flashlightLight.enabled = isOn;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
        }
    }
}

