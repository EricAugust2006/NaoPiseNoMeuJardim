using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public string nomeDoItem;
    private Inventario inventario;

    private void Start()
    {
        inventario = FindObjectOfType<Inventario>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player")){
            inventario.AdicionarItem(nomeDoItem);
            gameObject.SetActive(false);
        }   
    }
}
