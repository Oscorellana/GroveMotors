using UnityEngine;

/// <summary>
/// Pulses the emission color of this object's material to create a glow effect on pickup items.
/// Creates a per-instance material copy to avoid affecting other objects sharing the same material.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PickupGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float emissionStrength = Mathf.Lerp(minIntensity, maxIntensity, t);
        mat.SetColor("_EmissionColor", glowColor * Mathf.LinearToGammaSpace(emissionStrength));
    }
}
