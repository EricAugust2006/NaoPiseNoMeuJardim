using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VoltarPrimeiroAndar : MonoBehaviour
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
            transicaoDeCenas.CarregarCena("primeiroAndar");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = true;
            readyToSpeak = true;
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            readyToSpeak = false;
            botaoInteracao.SetActive(false);
        }
    }
}
