using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JARDIM : MonoBehaviour, IInteractable
{
    public bool IniciarJogo = false;
    private ScriptMae Mae;
    public GameObject BotaoInciar;

    private void Start()
    {
        Mae = FindObjectOfType<ScriptMae>();

    }

    public void Interact()
    {
        Conversar();
    }

    private void Conversar()
    {
        if (!IniciarJogo)
        {
            Debug.Log("PISOU NO JARDIM");
            IniciarJogo = true;

            if (IniciarJogo)
            {
                Mae.PerseguirFilho();
            }


        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Voc  pisou no jardim");
            BotaoInciar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BotaoInciar.SetActive(false);
        }
    }
}
