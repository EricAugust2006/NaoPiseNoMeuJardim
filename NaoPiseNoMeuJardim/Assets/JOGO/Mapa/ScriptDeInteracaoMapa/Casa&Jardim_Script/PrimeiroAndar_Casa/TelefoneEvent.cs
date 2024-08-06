using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelefoneEvent : MonoBehaviour
{
    public GameObject BotaoInteracao;
    public GameObject telefoneEventoEntrar;
    public GameObject telefoneEventoSair;
    private ScriptPersonagem personagemScript;

    public bool EventoIniciado = false;

    private void Start()
    {
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEventoEntrar.SetActive(false);
    }

    private void Update()
    {
        EventoTelefone();
    }

    public void EventoTelefone()
    {
        if (EventoIniciado && Input.GetKeyDown(KeyCode.E))
        {
            telefoneEventoEntrar.SetActive(true);

            if (EventoIniciado)
            {
                personagemScript.DesativarAnimacoes();
                personagemScript.enabled = false;
            }
        }

        if(EventoIniciado && Input.GetKeyDown(KeyCode.Escape)){
            telefoneEventoEntrar.SetActive(false);
            personagemScript.enabled = true;
            personagemScript.RestaurarAnimacoes();
        }
    }

    public void SairDoEvento()
    {
        telefoneEventoEntrar.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventoIniciado = true;
            BotaoInteracao.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventoIniciado = false;
            BotaoInteracao.SetActive(false);
        }
    }
}
