using UnityEngine;

/// <summary>
/// Camera post-processing effect that overlays TV-static noise whose intensity scales
/// with how close the monster is to the player. Attach this to the player camera.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class StaticNoiseEffect : MonoBehaviour
{
    [Header("Monster Reference")]
    [Tooltip("Transform of the monster/enemy that triggers the effect.")]
    public Transform monster;

    [Header("Distance Thresholds")]
    [Tooltip("Distance at which the static begins to appear.")]
    public float startDistance = 15f;

    [Tooltip("Distance at which the static reaches full intensity.")]
    public float fullIntensityDistance = 3f;

    [Header("Intensity")]
    [Tooltip("Maximum static strength when the monster is at fullIntensityDistance.")]
    [Range(0f, 1f)]
    public float maxIntensity = 0.85f;

    [Tooltip("How quickly the intensity ramps up and down (higher = snappier).")]
    public float smoothSpeed = 3f;

    private Material staticMaterial;
    private float currentIntensity;

    private const string ShaderName = "Hidden/StaticNoiseShader";

    private void Start()
    {
        InitialiseMaterial();
    }

    private void OnEnable()
    {
        InitialiseMaterial();
    }

    private void InitialiseMaterial()
    {
        if (staticMaterial != null) return;

        Shader shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            Debug.LogError($"[StaticNoiseEffect] Shader '{ShaderName}' not found. " +
                           "Make sure StaticNoiseShader.shader is in the project.");
            enabled = false;
            return;
        }

        staticMaterial = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (staticMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        float targetIntensity = CalculateTargetIntensity();
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * smoothSpeed);

        staticMaterial.SetFloat("_Intensity", currentIntensity);
        staticMaterial.SetFloat("_Time2", Time.time);

        Graphics.Blit(src, dest, staticMaterial);
    }

    /// <summary>
    /// Returns a [0, maxIntensity] value based on the monster's distance to this camera.
    /// Returns 0 when no monster is assigned.
    /// </summary>
    private float CalculateTargetIntensity()
    {
        if (monster == null) return 0f;

        float distance = Vector3.Distance(transform.position, monster.position);

        if (distance >= startDistance) return 0f;
        if (distance <= fullIntensityDistance) return maxIntensity;

        float t = 1f - Mathf.InverseLerp(fullIntensityDistance, startDistance, distance);
        return t * maxIntensity;
    }

    private void OnDisable()
    {
        if (staticMaterial != null)
        {
            DestroyImmediate(staticMaterial);
            staticMaterial = null;
        }
    }
}
