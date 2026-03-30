using UnityEngine;

/// <summary>
/// Makes this GameObject always face the main camera on the Y-axis (useful for NPC sprites and world-space UI).
/// </summary>
public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(cameraTransform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
