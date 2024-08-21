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

    [Header("Booleanos")]
    public bool EventoIniciado = false;
    public bool eventoLigado = false;

    [Header("Scripts")]
    private ScriptPersonagem personagemScript;
    private Inventario inventario; ////

    [Header("Text")]
    public Text numeroTelefone; ////

    private void Start()
    {
        //modificacao
        inventario = FindObjectOfType<Inventario>();
         if (inventario.VerificarItem("Número de Telefone"))
        {
            numeroTelefone.text = "Número salvo: 123-456-789";
        }
        else
        {
            numeroTelefone.text = "Nenhum número salvo.";
        }

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
