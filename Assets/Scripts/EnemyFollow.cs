using UnityEngine;

/// <summary>
/// Simple transform-based follow behaviour. Moves this GameObject toward <see cref="target"/> each frame,
/// locked to its current Y position and always facing the target.
/// </summary>
public class EnemyFollow : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform target;

    void Update()
    {
        if (target == null) return;

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
}
