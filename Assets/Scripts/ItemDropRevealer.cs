using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class ItemDropRevealer : MonoBehaviour
{
    [System.Serializable]
    public class DropMapping
    {
        [Tooltip("The name (or prefix) of the pickup object this rule applies to.")]
        public string pickupName;

        [Tooltip("If assigned, this scene object will be revealed when the pickup is dropped.")]
        public GameObject targetToReveal;

        [Tooltip("Optional spawn point for prefabs. Ignored if targetToReveal is a scene object.")]
        public Transform spawnPoint;
    }

    [Header("Drop Settings")]
    [SerializeField] private DropMapping[] dropMappings;
    [SerializeField] private bool disablePickupInsteadOfDestroy = false;
    [SerializeField] private string pickupLayerName = "Pickup";
    [SerializeField] private bool useStartsWithMatching = true;

    private int pickupLayer;

    private void Awake()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        pickupLayer = LayerMask.NameToLayer(pickupLayerName);

        // Hide all scene target objects at start
        foreach (var mapping in dropMappings)
        {
            if (mapping.targetToReveal != null && mapping.targetToReveal.scene.rootCount > 0)
                SetObjectVisible(mapping.targetToReveal, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != pickupLayer) return;

        foreach (var mapping in dropMappings)
        {
            bool nameMatch = useStartsWithMatching
                ? other.gameObject.name.StartsWith(mapping.pickupName, System.StringComparison.OrdinalIgnoreCase)
                : other.gameObject.name.Equals(mapping.pickupName, System.StringComparison.OrdinalIgnoreCase);

            if (nameMatch)
            {
                // Remove the pickup
                if (disablePickupInsteadOfDestroy)
                    other.gameObject.SetActive(false);
                else
                    Destroy(other.gameObject);

                // Reveal or spawn target
                if (mapping.targetToReveal != null)
                {
                    if (mapping.targetToReveal.scene.rootCount > 0)
                    {
                        // Scene object ? just make it visible
                        SetObjectVisible(mapping.targetToReveal, true);
                    }
                    else if (mapping.spawnPoint != null)
                    {
                        // Prefab ? instantiate at spawn point
                        GameObject spawned = Instantiate(mapping.targetToReveal, mapping.spawnPoint.position, mapping.spawnPoint.rotation);
                        spawned.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning($"DropMapping for {mapping.pickupName} has prefab target but no spawn point assigned.", this);
                    }
                }

                return; // stop after first match
            }
        }

        Debug.LogWarning($"No matching drop mapping found for {other.gameObject.name}", this);
    }

    private void SetObjectVisible(GameObject obj, bool visible)
    {
        obj.SetActive(true); // Ensure object is active

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
            r.enabled = visible;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (TryGetComponent(out BoxCollider box))
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Gizmos.DrawCube(transform.position, box.size);
        }

        // Draw spawn points for prefabs
        if (dropMappings != null)
        {
            foreach (var mapping in dropMappings)
            {
                if (mapping.spawnPoint != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(mapping.spawnPoint.position, 0.1f);
                }
            }
        }
    }
#endif
}
