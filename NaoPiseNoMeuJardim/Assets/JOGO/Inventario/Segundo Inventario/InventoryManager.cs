using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Inventory inventory = new Inventory();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Item é null. Não é possível adicionar.");
            return;
        }

        inventory.AddItem(item);
        Debug.Log("Item adicionado");
        // Verifica se o InventoryUI.Instance foi inicializado
        if (InventoryUI.instance != null)
        {
            Debug.Log("InventoryUI.Instance inicializado, atualizando a UI.");
            InventoryUI.instance.UpdateInventoryUI();
        }
        else
        {
            Debug.LogError("InventoryUI.Instance não foi encontrado. Verifique se o script InventoryUI está corretamente configurado na cena.");
        }
    }
}
