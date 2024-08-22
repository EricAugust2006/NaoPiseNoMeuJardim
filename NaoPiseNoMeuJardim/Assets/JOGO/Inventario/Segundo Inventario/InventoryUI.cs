using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public GameObject inventoryUI; // Referência ao painel do inventário
    public Transform itemParent;   // Local onde os slots de itens serão listados
    public GameObject itemSlotPrefab; // Prefab do slot de item

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            inventoryUI.SetActive(false); // Começa oculto
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory(); // Use o método ToggleInventory para garantir a atualização da UI
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
        if (isActive)
        {
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        Debug.Log("Updating Inventory UI...");
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in InventoryManager.Instance.inventory.items)
        {
            Debug.Log("Instantiating item slot for: " + item.itemName);
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemParent);
            itemSlot.GetComponentInChildren<Image>().sprite = item.itemIcon;
        }
    }
}
