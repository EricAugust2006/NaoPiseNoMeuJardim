using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortaSubirAndar : MonoBehaviour
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
        if (personagemScript != null)
        {
            personagemAnimator = personagemScript.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("ScriptPersonagem n�o encontrado na cena!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak && eventoLigado == true)
        {
            if (!startDialogue)
            {
                // Desativa pulo e anima��o de corrida
                personagemScript.speed = 0f;
                DesativarAnimacoes();
                StartDialogue();
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue();
                transicaoDeCenas.CarregarCena("SegundoAndar");
            }
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueNpc.Length)
        {
            StartCoroutine(showDialogue());
        }
        else
        {
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            personagemScript.speed = 6f;
            RestaurarAnimacoes();
        }
    }

    void StartDialogue()
    {
        nameNpc.text = "Isaque";
        imageNpc.sprite = spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(showDialogue());
    }

    IEnumerator showDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
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

    // M�todo para desativar anima��es
    void DesativarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Desativando anima��es");
            personagemAnimator.SetBool("Correndo", false);
            personagemAnimator.SetBool("Caindo", false);
            personagemAnimator.SetFloat("Velocidade", 0f);
            // Adicione qualquer outro par�metro que precise ser desativado
        }
    }

    // M�todo para restaurar anima��es
    void RestaurarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Restaurando anima��es");
            // Ajuste os valores conforme necess�rio para restaurar o estado anterior
            personagemAnimator.SetBool("Correndo", true); // Se necess�rio, ajuste conforme a l�gica do seu jogo
        }
    }

}