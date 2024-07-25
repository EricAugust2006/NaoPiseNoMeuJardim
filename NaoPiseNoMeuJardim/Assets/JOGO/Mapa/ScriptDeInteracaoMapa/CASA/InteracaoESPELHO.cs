using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracaoESPELHO : MonoBehaviour, IInteractable
{
    private bool Interagido = false;
    public GameObject botaoInterage;
    private ScriptPersonagem personagem;

    public void Interact()
    {
        Espelho();
    }

    private void Start()
    {
        personagem = FindObjectOfType<ScriptPersonagem>();
    }

    private void Espelho()
    {
        if (!Interagido)
        {
            Debug.Log("É você!");
            personagem.Empurrar();
            Interagido = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            botaoInterage.SetActive(false);
        }
    }
}
