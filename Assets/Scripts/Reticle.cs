using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Draws a small crosshair reticle at the center of the screen.
/// Optionally highlights when the player is looking at an interactable object.
/// </summary>
public class Reticle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Image reticleImage;

    [Header("Appearance")]
    [SerializeField] private Color defaultColor = new Color(1f, 1f, 1f, 0.85f);
    [SerializeField] private Color highlightColor = new Color(1f, 0.9f, 0.2f, 1f);

    [Header("Detection")]
    [SerializeField] private float maxDetectionDistance = 5f;
    [SerializeField] private LayerMask detectionMask = ~0;

    private int pickupLayer;

    private void Start()
    {
        pickupLayer = LayerMask.NameToLayer("Pickup");

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (reticleImage != null)
        {
            reticleImage.sprite = CreateCircleSprite();
            reticleImage.color = defaultColor;
        }
    }

    /// <summary>
    /// Generates a filled circle sprite at runtime so no external asset is required.
    /// </summary>
    private static Sprite CreateCircleSprite()
    {
        const int TextureSize = 64;
        const float Radius = TextureSize * 0.5f;

        Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[TextureSize * TextureSize];

        Vector2 center = new Vector2(Radius, Radius);
        for (int y = 0; y < TextureSize; y++)
        {
            for (int x = 0; x < TextureSize; x++)
            {
                float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                // Smooth the edge over 1 pixel for anti-aliasing
                float alpha = 1f - Mathf.Clamp01(distance - (Radius - 1.5f));
                pixels[y * TextureSize + x] = new Color(1f, 1f, 1f, alpha);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, TextureSize, TextureSize),
            new Vector2(0.5f, 0.5f)
        );
    }

    private void Update()
    {
        if (playerCamera == null || reticleImage == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        bool hitInteractable = Physics.Raycast(ray, out RaycastHit hit, maxDetectionDistance, detectionMask)
                               && hit.collider.gameObject.layer == pickupLayer;

        reticleImage.color = hitInteractable ? highlightColor : defaultColor;
    }
}
