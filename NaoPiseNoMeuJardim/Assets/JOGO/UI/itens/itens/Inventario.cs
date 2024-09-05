using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Inventario : MonoBehaviour
{
    public List<Item> itens = new List<Item>();
    public event Action<Item> OnItemAdicionado;

    public void AdicionarItem(Item item)
    {
        if (!itens.Contains(item))
        {
            itens.Add(item);
            Debug.Log("Item coletado: " + item.nomeItem);
            OnItemAdicionado?.Invoke(item); // Notifica a UI que um item foi adicionado
        }
    }
}
