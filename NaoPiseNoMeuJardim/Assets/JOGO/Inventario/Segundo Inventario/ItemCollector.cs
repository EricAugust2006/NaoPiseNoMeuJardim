using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public Item itemToCollect; // Arraste o prefab do item aqui no Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(itemToCollect);
            }
            else
            {
                Debug.LogError("InventoryManager.Instance não foi encontrado. Verifique se o script InventoryManager está corretamente configurado na cena.");
            }
            Destroy(gameObject); // Remove o item da cena após a coleta
        }
    }
}
