using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelefoneEvent : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject BotaoInteracao;
    public GameObject telefoneEventoEntrar;
    public GameObject telefoneEventoSair;

    [Header("Script")]
    private ScriptPersonagem personagemScript;

    [Header("Booleanos")]
    public bool EventoIniciado = false;
    public bool eventoLigado = false;

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
        if (EventoIniciado && Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
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
            eventoLigado = true;
            EventoIniciado = true;
            BotaoInteracao.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            EventoIniciado = false;
            BotaoInteracao.SetActive(false);
        }
    }
}
