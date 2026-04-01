using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pulses the emission color of this object's material (and all child renderers) to create
/// a glow effect on pickup items. Creates per-instance material copies to avoid affecting
/// other objects sharing the same material.
/// </summary>
public class PickupGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    private readonly List<Material> instancedMaterials = new List<Material>();

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
    private const string EmissionKeyword = "_EMISSION";

    void Start()
    {
        // Collect renderers on this object and all children.
        Renderer[] renderers = GetComponentsInChildren<Renderer>(includeInactive: true);

        foreach (Renderer r in renderers)
        {
            // .materials returns per-instance copies automatically.
            foreach (Material m in r.materials)
            {
                m.EnableKeyword(EmissionKeyword);
                instancedMaterials.Add(m);
            }
        }
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float emissionStrength = Mathf.Lerp(minIntensity, maxIntensity, t);
        Color emission = glowColor * Mathf.LinearToGammaSpace(emissionStrength);

        foreach (Material m in instancedMaterials)
            m.SetColor(EmissionColorId, emission);
    }

    void OnDestroy()
    {
        // Clean up instanced material copies to avoid memory leaks.
        foreach (Material m in instancedMaterials)
        {
            if (m != null)
                Destroy(m);
        }
        instancedMaterials.Clear();
    }
}
