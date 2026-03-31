using UnityEngine;

/// <summary>
/// Simple transform-based follow behaviour. Moves this GameObject toward <see cref="target"/> each frame,
/// locked to its current Y position and always facing the target.
/// Freezes movement while the player's flashlight is actively illuminating this enemy.
/// </summary>
public class EnemyFollow : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform target;

    [Header("Flashlight Freeze")]
    [Tooltip("The spotlight used as the player's flashlight.")]
    public Light flashlight;

    void Update()
    {
        if (target == null) return;

        if (IsIlluminatedByFlashlight())
            return;

        // Keep destination at the monster's own Y so it never floats or sinks.
        Vector3 destination = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Rotate to face the player on the horizontal plane only.
        Vector3 lookDirection = destination - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    /// <summary>
    /// Returns true when the flashlight is on, this enemy is within its cone and range,
    /// and no geometry is blocking the line of sight from the light to this enemy.
    /// </summary>
    private bool IsIlluminatedByFlashlight()
    {
        if (flashlight == null || !flashlight.enabled)
            return false;

        Vector3 toEnemy = transform.position - flashlight.transform.position;
        float distance = toEnemy.magnitude;

        if (distance > flashlight.range)
            return false;

        float angle = Vector3.Angle(flashlight.transform.forward, toEnemy);
        if (angle > flashlight.spotAngle * 0.5f)
            return false;

        // Raycast to confirm no geometry blocks the light.
        if (Physics.Raycast(flashlight.transform.position, toEnemy.normalized, out RaycastHit hit, distance))
        {
            // The ray hit something — only frozen if it hit this enemy (or a child of it).
            return hit.transform.IsChildOf(transform) || hit.transform == transform;
        }

        return true;
    }
}
