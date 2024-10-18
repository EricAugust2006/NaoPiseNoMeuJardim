using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntrarNoQuarto : MonoBehaviour
{
    [Header("Dialogo")]
    public string[] dialogueNpc;
    public int dialogueIndex;

    [Header("UI Dialogo")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Elementos para Dialogo")]
    public TextMeshProUGUI nameNpc;
    public Image imageNpc;
    public Sprite spriteNpc;

    [Header("Booleanos")]
    public bool readyToSpeak;
    public bool startDialogue;
    public bool eventoLigado = false;

    [Header("Script e Animator personagem")]
    private ScriptPersonagem personagemScript;
    private Animator personagemAnimator;

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
            transicaoDeCenas.CarregarCena("meuQuarto");
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
