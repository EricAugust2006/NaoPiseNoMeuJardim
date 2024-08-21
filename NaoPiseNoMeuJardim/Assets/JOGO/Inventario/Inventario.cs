using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{

    public List<Item> itens; //lista que armazonae todos os itens do inv

    private void Start()
    {
        //carrega inv salvo, se tiver
        CarregarInventario();
    }
    
    public void AdicionarItem(string nomeDoItem)
    {
        foreach(var item in itens)
        {
            if(item.nome == nomeDoItem)
            {
                item.coletado = true;
                SalvarInventario();
                Debug.Log(nomeDoItem + " foi adicionado ao inventário.");
                break;
            }
        }
    }

    public bool VerificarItem(string nomeDoItem)
    {
        foreach(var item in itens)
        {
            if(item.nome == nomeDoItem && item.coletado)
            {
                return true;
            }
        }
        return false;
    }

    private void SalvarInventario()
    {
        foreach(var item in itens)
        {
            PlayerPrefs.SetInt(item.nome, item.coletado ? 1 : 0);
        }    
        PlayerPrefs.Save();
    }

    private void CarregarInventario()
    {
        foreach(var item in itens)
        {
            item.coletado = PlayerPrefs.GetInt(item.nome, 0) == 1;
        }
    }
}
