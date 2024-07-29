using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JARDIM : MonoBehaviour, IInteractable
{
    public bool IniciarJogo = false;
    public GameObject BotaoInciar;
    private ScriptMae Mae;
    private bool playerInJardim = false;

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
                Mae.GetComponent<Collider2D>().isTrigger = false; 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Você pisou no jardim");
            playerInJardim = true;
            BotaoInciar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInJardim = false;
            BotaoInciar.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInJardim && Input.GetKeyDown(KeyCode.E))
        {
            Mae.PerseguirFilho();
            Mae.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
