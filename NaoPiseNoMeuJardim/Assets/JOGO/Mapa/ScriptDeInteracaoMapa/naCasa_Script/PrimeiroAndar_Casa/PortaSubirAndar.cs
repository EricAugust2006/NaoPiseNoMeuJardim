using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortaSubirAndar : MonoBehaviour
{

    [Header("UI Dialogo")]
    public GameObject dialoguePanel;

    [Header("Booleanos")]
    public bool eventoLigado = false;

    [Header("Script e Animator personagem")]
    private ScriptPersonagem personagemScript;

    [Header("GameObjects e Script TransicaoCenas")]
    public GameObject botaoInteracao;
    public TransicaoDeCenas transicaoDeCenas;

    void Start()
    {
        dialoguePanel.SetActive(false);
        personagemScript = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
        {
            transicaoDeCenas.CarregarCena("SegundoAndar");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = true;
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            botaoInteracao.SetActive(false);
        }
    }
}