using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PickupGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    private Renderer rend;
    private Material mat;
    private Color baseEmission;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Get the instance of the material so we don’t affect other objects using the same one
        mat = rend.material;

        // Make sure emission is enabled
        mat.EnableKeyword("_EMISSION");

        // Store base emission color
        baseEmission = glowColor;
    }

    void Update()
    {
        // Calculate pulsing intensity
        float emissionStrength = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        Color emissionColor = baseEmission * Mathf.LinearToGammaSpace(emissionStrength);

        // Apply the color to the material
        mat.SetColor("_EmissionColor", emissionColor);
    }
}

