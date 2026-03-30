using UnityEngine;

/// <summary>
/// Raycast-based item pickup. Press E while looking at a pickup object within range
/// to add it to the inventory. Object names must end with " PICKUP" (e.g. "Key PICKUP").
/// Shows a pickup prompt UI when a valid pickup is in range and in view.
/// </summary>
public class PickupClass : MonoBehaviour
{
    private const string PickupSuffix = " PICKUP";
    private const string FlashlightName = "Flashlight";

    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float pickupRange = 3f;
    [SerializeField] private InventorySystem inventorySystem;

    [Header("Pickup Prompt")]
    [SerializeField] private GameObject pickupPrompt;

    void Update()
    {
        UpdatePromptVisibility();

        if (Input.GetKeyDown(KeyCode.E))
            TryPickup();
    }

    /// <summary>Shows the pickup prompt when a valid pickup object is in the crosshair.</summary>
    private void UpdatePromptVisibility()
    {
        if (pickupPrompt == null) return;

        bool show = IsLookingAtPickup();
        if (pickupPrompt.activeSelf != show)
            pickupPrompt.SetActive(show);
    }

    private bool IsLookingAtPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, pickupRange, pickupLayer))
            return false;

        string itemName = hitInfo.collider.gameObject.name.Replace(PickupSuffix, "").Trim();
        return !string.IsNullOrEmpty(itemName) && itemName != FlashlightName;
    }

    private void TryPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, pickupRange, pickupLayer)) return;

        string itemName = hitInfo.collider.gameObject.name.Replace(PickupSuffix, "").Trim();

        if (string.IsNullOrEmpty(itemName)) return;
        if (itemName == FlashlightName) return;

        bool added = inventorySystem.AddItem(itemName);
        if (added)
            hitInfo.collider.gameObject.SetActive(false);
        else
            Debug.Log("Pickup failed - inventory full: " + itemName);
    }
}






// Old Script for PickupClass
/*public class PickupClass : MonoBehaviour
{

    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float ThrowingForce;
    [SerializeField] private float PickupRange;
    [SerializeField] private Transform Hand;

    private Rigidbody CurrentObjectRigidbody;
    private Collider CurrentObjectCollider;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray Pickupray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

            if (Physics.Raycast(Pickupray, out RaycastHit hitInfo, PickupRange, PickupLayer))
            {
                if (CurrentObjectRigidbody)
                {
                    CurrentObjectRigidbody.isKinematic = false;
                    CurrentObjectCollider.enabled = true;

                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;

                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;
                }
                else
                {
                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;

                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;

                }

                return;
            }
            if (CurrentObjectRigidbody)
            {
                CurrentObjectRigidbody.isKinematic = false;
                CurrentObjectCollider.enabled = true;

                CurrentObjectRigidbody = null;
                CurrentObjectCollider = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentObjectRigidbody.isKinematic = false;
            CurrentObjectCollider.enabled = true;

            CurrentObjectRigidbody.AddForce(PlayerCamera.transform.forward * ThrowingForce, ForceMode.Impulse);

            CurrentObjectRigidbody = null;
            CurrentObjectCollider = null;
        }

        if (CurrentObjectRigidbody)
        {
            CurrentObjectRigidbody.position = Hand.position;
            CurrentObjectRigidbody.rotation = Hand.rotation;
        }
        
    }
}
*/