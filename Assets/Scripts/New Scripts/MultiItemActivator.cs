using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MultiItemActivator : MonoBehaviour
{
    [System.Serializable]
    public class ItemActivation
    {
        public string itemName;            // must match the inventory item name
        public GameObject objectToActivate; // what to turn on when item used
        public bool consumeItem;           // remove item after use
        public bool onlyActivateOnce = true;
        [HideInInspector] public bool hasActivated = false;
    }

    [Header("Setup")]
    public ItemActivation[] itemActivations; // assign all item-object pairs here

    private bool playerInRange = false;
    private InventorySystem playerInventory;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponentInChildren<InventorySystem>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    private void Update()
    {
        if (!playerInRange || playerInventory == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryActivateItem();
        }
    }

    private void TryActivateItem()
    {
        string heldItem = playerInventory.GetSelectedItem();
        if (string.IsNullOrEmpty(heldItem))
        {
            Debug.Log("No item selected.");
            return;
        }

        foreach (ItemActivation act in itemActivations)
        {
            if (act.itemName == heldItem)
            {
                if (act.onlyActivateOnce && act.hasActivated)
                {
                    Debug.Log($"{heldItem} already used here.");
                    return;
                }

                // Activate the target object
                if (act.objectToActivate != null)
                {
                    act.objectToActivate.SetActive(true);
                    Debug.Log($"Activated {act.objectToActivate.name} using {heldItem}!");
                }

                // Mark the item as used in inventory (turns green, hides hand model)
                playerInventory.MarkSlotUsed(heldItem);

                // Optionally remove item entirely if consumeItem is checked
                if (act.consumeItem)
                {
                    playerInventory.RemoveItem(heldItem);
                }

                act.hasActivated = true;
                return;
            }
        }

        Debug.Log($"No matching activation for {heldItem} in this trigger.");
    }
}




// working script as of 12:51pm Oct 14,2025
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MultiItemActivator : MonoBehaviour
{
    [System.Serializable]
    public class ItemActivation
    {
        public string itemName;           // must match the inventory item name
        public GameObject objectToActivate; // what to turn on when item used
        public bool consumeItem;          // should we remove item after use
        public bool onlyActivateOnce = true;
        [HideInInspector] public bool hasActivated = false;
    }

    [Header("Setup")]
    public ItemActivation[] itemActivations; // assign all your item-object pairs here

    private bool playerInRange = false;
    private InventorySystem playerInventory;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponentInChildren<InventorySystem>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    private void Update()
    {
        if (!playerInRange || playerInventory == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryActivateItem();
        }
    }

    private void TryActivateItem()
    {
        string heldItem = playerInventory.GetSelectedItem();
        if (string.IsNullOrEmpty(heldItem))
        {
            Debug.Log("No item selected.");
            return;
        }

        foreach (ItemActivation act in itemActivations)
        {
            if (act.itemName == heldItem)
            {
                if (act.onlyActivateOnce && act.hasActivated)
                {
                    Debug.Log($"{heldItem} already used here.");
                    return;
                }

                if (act.objectToActivate != null)
                {
                    act.objectToActivate.SetActive(true);
                    Debug.Log($"Activated {act.objectToActivate.name} using {heldItem}!");
                }

                if (act.consumeItem)
                {
                    playerInventory.RemoveItem(heldItem);
                }

                act.hasActivated = true;
                return;
            }
        }

        Debug.Log($"No matching activation for {heldItem} in this trigger.");
    }
}
*/