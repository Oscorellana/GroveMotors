using UnityEngine;

/// <summary>
/// Toggles the assigned spotlight on and off with a configurable key (default: L).
/// Uses L instead of F to avoid conflicting with <see cref="NPCSystem"/> dialogue input.
/// </summary>
public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;
    public KeyCode toggleKey = KeyCode.L;

    private bool isOn = true;

    void Start()
    {
        if (flashlightLight == null)
            flashlightLight = GetComponentInChildren<Light>();

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
