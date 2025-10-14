using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float PickupRange = 3f;
    [SerializeField] private InventorySystem inventorySystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryPickup();
    }

    void TryPickup()
    {
        Ray ray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, PickupRange, PickupLayer))
        {
            string fullName = hitInfo.collider.gameObject.name;
            string itemName = fullName.Replace(" PICKUP", "").Trim();

            if (string.IsNullOrEmpty(itemName)) return;
            if (itemName == "Flashlight") return; // flashlight is permanent in slot 1

            bool added = inventorySystem.AddItem(itemName);
            if (added)
            {
                hitInfo.collider.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Pickup failed — inventory full or duplicate: " + itemName);
            }
        }
    }
}



// working code for inventory, but not fully working
/*using UnityEngine;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float PickupRange = 3f;
    [SerializeField] private InventorySystem inventorySystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, PickupRange, PickupLayer))
        {
            string itemName = hitInfo.collider.gameObject.name.Replace(" PICKUP", "");
            if (itemName == "Flashlight") return; // Skip flashlight

            inventorySystem.AddItem(itemName);

            // Hide pickup in world
            hitInfo.collider.gameObject.SetActive(false);
        }
    }
}
*/






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