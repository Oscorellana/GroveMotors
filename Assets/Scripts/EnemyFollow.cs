using UnityEngine;

/// <summary>
/// Simple transform-based follow behaviour. Moves this GameObject toward <see cref="target"/> each frame.
/// </summary>
public class EnemyFollow : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform target;

    void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
