using UnityEngine;
using UnityEngine.UI;

public class RemoveInventoryUIFromChildren : MonoBehaviour
{
    void Start()
    {
        // Itera sobre todos os filhos deste GameObject
        foreach (Transform child in transform)
        {
            // Verifica se o filho tem o componente InventoryUI
            InventarioUI inventoryUI = child.GetComponent<InventarioUI>();

            // Se o filho tem o componente, ele será destruído/removido
            if (inventoryUI != null)
            {
                Destroy(inventoryUI);
            }
        }
    }
}
