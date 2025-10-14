using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI[] slotTexts;      // 6 TMP text fields
    public Image[] slotHighlights;           // Slot1Highlight … Slot6Highlight
    public Image[] slotUsedHighlights;       // Slot1UsedHighlight … Slot6UsedHighlight

    [Header("Item Database")]
    public ItemData[] itemDatabase;          // assign every possible item and its hand model here

    private List<string> inventory;          // length-fixed to 6
    private List<GameObject> handInventory;  // parallel list of models (length-fixed to 6)
    private bool[] usedSlots;                // track which slots are marked used (green)

    private int selectedIndex = 0;           // default select flashlight (slot 0)
    private const int SLOT_COUNT = 6;

    private void Awake()
    {
        inventory = new List<string>(SLOT_COUNT);
        handInventory = new List<GameObject>(SLOT_COUNT);
        usedSlots = new bool[SLOT_COUNT];

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            inventory.Add("");        // default empty
            handInventory.Add(null);  // no model assigned yet
            usedSlots[i] = false;
        }
    }

    private void Start()
    {
        // Disable all hand models initially
        foreach (var d in itemDatabase)
        {
            if (d != null && d.handModel != null)
                d.handModel.SetActive(false);
        }

        // Put flashlight in slot 0 if present
        GameObject flashlightModel = FindModelByName("Flashlight");
        if (flashlightModel == null)
            Debug.LogWarning("No Flashlight model found in itemDatabase!");

        inventory[0] = "Flashlight";
        handInventory[0] = flashlightModel;

        UpdateUI();
        UpdateHandItems();
    }

    private void Update()
    {
        // number key selection 1..6
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectItem(i);
        }
    }

    public bool AddItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return false;

        int slotToUse = -1;
        for (int i = 1; i < SLOT_COUNT; i++) // slot 0 reserved for flashlight
        {
            if (string.IsNullOrEmpty(inventory[i]))
            {
                slotToUse = i;
                break;
            }
        }

        if (slotToUse == -1)
        {
            Debug.Log("Inventory full! Could not add: " + itemName);
            return false;
        }

        GameObject model = FindModelByName(itemName);

        inventory[slotToUse] = itemName;
        handInventory[slotToUse] = model;

        if (model != null)
            model.SetActive(false);

        Debug.Log($"Added {itemName} to slot {slotToUse}");
        UpdateUI();
        return true;
    }

    public void RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return;

        for (int i = 1; i < SLOT_COUNT; i++)
        {
            if (inventory[i] == itemName)
            {
                if (handInventory[i] != null)
                    handInventory[i].SetActive(false);

                inventory[i] = "";
                handInventory[i] = null;
                usedSlots[i] = false;

                if (selectedIndex == i)
                    SelectItem(0);

                UpdateUI();
                return;
            }
        }
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= SLOT_COUNT) return;

        selectedIndex = index;
        UpdateUI();
        UpdateHandItems();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            // Update text
            if (slotTexts != null && i < slotTexts.Length && slotTexts[i] != null)
                slotTexts[i].text = string.IsNullOrEmpty(inventory[i]) ? "[Empty]" : inventory[i];

            // White selection highlight
            if (slotHighlights != null && i < slotHighlights.Length && slotHighlights[i] != null)
            {
                Color c = slotHighlights[i].color;
                c.a = (i == selectedIndex) ? 0.25f : 0f;
                slotHighlights[i].color = c;
                slotHighlights[i].gameObject.SetActive(true);
            }

            // Green used highlight
            if (slotUsedHighlights != null && i < slotUsedHighlights.Length && slotUsedHighlights[i] != null)
            {
                Color c = slotUsedHighlights[i].color;
                c.a = usedSlots[i] ? 0.25f : 0f;
                slotUsedHighlights[i].color = c;
                slotUsedHighlights[i].gameObject.SetActive(true);
            }
        }
    }

    private void UpdateHandItems()
    {
        // Hide all models
        foreach (var d in itemDatabase)
        {
            if (d != null && d.handModel != null)
                d.handModel.SetActive(false);
        }

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (handInventory[i] != null)
                handInventory[i].SetActive(false);
        }

        // Show selected item if not used
        if (selectedIndex >= 0 && selectedIndex < SLOT_COUNT)
        {
            GameObject model = handInventory[selectedIndex];
            if (!string.IsNullOrEmpty(inventory[selectedIndex]) && model != null && !usedSlots[selectedIndex])
                model.SetActive(true);
        }
    }

    private GameObject FindModelByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;

        foreach (var d in itemDatabase)
        {
            if (d != null && d.itemName == itemName)
                return d.handModel;
        }

        return null;
    }

    public string GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < SLOT_COUNT)
            return inventory[selectedIndex];
        return null;
    }

    public void MarkSlotUsed(string itemName)
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (inventory[i] == itemName)
            {
                usedSlots[i] = true;
                if (handInventory[i] != null)
                    handInventory[i].SetActive(false);
                break;
            }
        }
        UpdateUI();
    }

    // Optional helper
    public bool IsSlotUsed(int index)
    {
        if (index < 0 || index >= SLOT_COUNT) return false;
        return usedSlots[index];
    }
}





// old code as of 12:50 Oct 14, 2025
/*using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI[] slotTexts;      // 6 TMP text fields
    public Image[] slotHighlights;           // 6 highlight images (optional)

    [Header("Item Database")]
    public ItemData[] itemDatabase;          // assign every possible item and its hand model here

    private List<string> inventory;          // length-fixed to 6
    private List<GameObject> handInventory;  // parallel list of models (length-fixed to 6)

    private int selectedIndex = 0;           // default select flashlight (slot 0)
    private const int SLOT_COUNT = 6;

    private void Awake()
    {
        // initialize lists to fixed size
        inventory = new List<string>(SLOT_COUNT);
        handInventory = new List<GameObject>(SLOT_COUNT);

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            inventory.Add("");        // default empty
            handInventory.Add(null);  // no model assigned yet
        }
    }

    private void Start()
    {
        // Ensure itemDatabase models are all disabled initially
        foreach (var d in itemDatabase)
        {
            if (d != null && d.handModel != null)
                d.handModel.SetActive(false);
        }

        // Put flashlight in slot 0 if present in database
        GameObject flashlightModel = FindModelByName("Flashlight");
        if (flashlightModel == null)
        {
            Debug.LogWarning("No Flashlight model found in itemDatabase. Make sure you added an ItemData entry with itemName = \"Flashlight\".");
        }

        // Fill slot 0 with flashlight (overwrites whatever was there)
        inventory[0] = "Flashlight";
        handInventory[0] = flashlightModel;

        UpdateUI();
        UpdateHandItems();
    }

    private void Update()
    {
        // number key selection 1..6
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectItem(i);
        }
    }

    // Returns true if item added to inventory
    public bool AddItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return false;

        // Avoid adding duplicates (optional, uncomment if desired)
        // if (inventory.Contains(itemName)) return false;

        // Find first empty slot starting from index 1 (slot 0 reserved for flashlight)
        int slotToUse = -1;
        for (int i = 1; i < SLOT_COUNT; i++)
        {
            if (string.IsNullOrEmpty(inventory[i]))
            {
                slotToUse = i;
                break;
            }
        }

        if (slotToUse == -1)
        {
            Debug.Log("Inventory full! Could not add: " + itemName);
            return false;
        }

        // Find matching model (if any)
        GameObject model = FindModelByName(itemName);

        inventory[slotToUse] = itemName;
        handInventory[slotToUse] = model;

        // Ensure the model is inactive in-hand until selected
        if (model != null)
            model.SetActive(false);

        Debug.Log($"Added {itemName} to inventory slot {slotToUse}.");
        UpdateUI();
        return true;
    }

    public void RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return;

        for (int i = 1; i < SLOT_COUNT; i++)
        {
            if (inventory[i] == itemName)
            {
                // deactivate model if it's active
                if (handInventory[i] != null)
                    handInventory[i].SetActive(false);

                inventory[i] = "";
                handInventory[i] = null;
                Debug.Log($"Removed {itemName} from slot {i}.");
                UpdateUI();
                // if we removed currently selected, select flashlight (0)
                if (selectedIndex == i)
                {
                    SelectItem(0);
                }
                return;
            }
        }
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= SLOT_COUNT) return;

        // Only allow selecting an empty slot — still shows [Empty] but no model
        selectedIndex = index;
        Debug.Log($"Selected slot {index} -> {(string.IsNullOrEmpty(inventory[index]) ? "[Empty]" : inventory[index])}");
        UpdateUI();
        UpdateHandItems();
    }

    private void UpdateUI()
    {
        // ensure arrays are correctly set in inspector
        if (slotTexts == null || slotTexts.Length < SLOT_COUNT)
        {
            // non-fatal: just skip UI update if not wired up
            // But log once to help debugging
#if UNITY_EDITOR
            Debug.LogWarning("slotTexts not assigned or size < 6. Assign 6 TMP Text objects in inspector.");
#endif
        }

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (slotTexts != null && i < slotTexts.Length && slotTexts[i] != null)
                slotTexts[i].text = string.IsNullOrEmpty(inventory[i]) ? "[Empty]" : inventory[i];

            if (slotHighlights != null && i < slotHighlights.Length && slotHighlights[i] != null)
                slotHighlights[i].gameObject.SetActive(i == selectedIndex);
        }
    }

    private void UpdateHandItems()
    {
        // Hide all known models from the itemDatabase (defensive)
        foreach (var d in itemDatabase)
        {
            if (d != null && d.handModel != null)
                d.handModel.SetActive(false);
        }

        // Also hide any model stored in handInventory to be safe
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            var m = handInventory[i];
            if (m != null)
                m.SetActive(false);
        }

        // Show selected model if the slot contains an item and model exists
        if (selectedIndex >= 0 && selectedIndex < SLOT_COUNT)
        {
            GameObject model = handInventory[selectedIndex];
            if (!string.IsNullOrEmpty(inventory[selectedIndex]) && model != null)
            {
                model.SetActive(true);
            }
            else
            {
                // nothing to show for this slot (empty or no model)
            }
        }
    }

    private GameObject FindModelByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;

        foreach (var d in itemDatabase)
        {
            if (d == null) continue;
            if (d.itemName == itemName)
            {
                return d.handModel;
            }
        }
        // not found
        return null;
    }

    public string GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < SLOT_COUNT)
            return inventory[selectedIndex];
        return null;
    }
}
*/






//old code
/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI[] slotTexts;
    public Image[] slotHighlights;

    [Header("Item Models in Hand")]
    public GameObject[] handItems;
    // 6 models, in the same order as slots: Flashlight, Item1, Item2, Item3, Item4, Item5

    private List<string> inventory = new List<string>();
    private int selectedIndex = 0;

    private void Start()
    {
        // Flashlight always first
        inventory.Add("Flashlight");

        // Fill rest
        for (int i = 1; i < 6; i++)
            inventory.Add("");

        UpdateUI();
        UpdateHandItems();
    }

    private void Update()
    {
        // Number key selection
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectItem(i);
            }
        }
    }

    public void AddItem(string itemName)
    {
        // Add to first empty slot (after flashlight)
        for (int i = 1; i < inventory.Count; i++)
        {
            if (string.IsNullOrEmpty(inventory[i]))
            {
                inventory[i] = itemName;
                Debug.Log($"Added {itemName} to inventory.");
                UpdateUI();
                return;
            }
        }

        Debug.Log("Inventory full!");
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= inventory.Count)
            return;

        selectedIndex = index;
        Debug.Log($"Selected: {inventory[selectedIndex]}");

        UpdateUI();
        UpdateHandItems();
    }

    public string GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.Count)
            return inventory[selectedIndex];
        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (slotTexts != null && i < slotTexts.Length)
                slotTexts[i].text = string.IsNullOrEmpty(inventory[i]) ? "[Empty]" : inventory[i];

            if (slotHighlights != null && i < slotHighlights.Length)
                slotHighlights[i].gameObject.SetActive(i == selectedIndex);
        }
    }

    private void UpdateHandItems()
    {
        // Hide all models
        foreach (GameObject item in handItems)
        {
            if (item != null)
                item.SetActive(false);
        }

        // Show current selected model if it exists
        if (handItems != null && selectedIndex < handItems.Length)
        {
            GameObject selectedModel = handItems[selectedIndex];
            if (selectedModel != null && !string.IsNullOrEmpty(inventory[selectedIndex]))
                selectedModel.SetActive(true);
        }
    }
}
*/