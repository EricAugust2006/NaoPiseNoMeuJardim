using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelefoneEvent : MonoBehaviour
{
    public GameObject BotaoInteracao;
    public GameObject telefoneEvento;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BotaoInteracao.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BotaoInteracao.SetActive(false);
        }
    }
}
