using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventarioUI : MonoBehaviour
{
   public GameObject slotPrefab; //prefab do slot com a imagem do item
   public Transform itemParent; //referencia ao pai onde os icones dos itens serão adicionados

    private Inventario inventario;  

    private void Start() {
        inventario = FindObjectOfType<Inventario>();
        inventario.OnItemAdicionado += AtualizarUI;
    }

    private void AtualizarUI(Item item)
    {  
        GameObject slot = Instantiate(slotPrefab, itemParent);
        Image itemIcon = slot.GetComponent<Image>();
        if(itemIcon != null)
        {
            itemIcon.sprite = item.iconeItem;
        }
    }
}
