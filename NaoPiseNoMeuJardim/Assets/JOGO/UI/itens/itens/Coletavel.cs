using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player"))
        {
            Inventario inventario = collision.GetComponent<Inventario>();
            if(inventario != null){
                inventario.AdicionarItem(item);
                Destroy(gameObject);
            }
        }
    }
}
