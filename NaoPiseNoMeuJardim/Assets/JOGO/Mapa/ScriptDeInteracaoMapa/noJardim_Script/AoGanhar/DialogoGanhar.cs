using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoGanhar : MonoBehaviour
{
   public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI nameNpc;
    public Image imageNpc;
    public Sprite spriteNpc;

    public bool readyToSpeak;
    public bool startDialogue;

    private Animator personagemAnimator;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak)
        {
            if (!startDialogue)
            {
                // Desativa pulo e animação de corrida
                DesativarAnimacoes();
                StartDialogue();
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue();
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
            RestaurarAnimacoes();
        }
    }

    void StartDialogue()
    {
        nameNpc.text = "...";
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
            yield return new WaitForSeconds(0.04f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
        }
    }

    // Método para desativar animações
    void DesativarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Desativando animações");
            personagemAnimator.SetBool("Correndo", false);
            personagemAnimator.SetBool("Caindo", false);
            personagemAnimator.SetFloat("Velocidade", 0f);
            // Adicione qualquer outro parâmetro que precise ser desativado
        }
    }

    // Método para restaurar animações
    void RestaurarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Restaurando animações");
            // Ajuste os valores conforme necessário para restaurar o estado anterior
            personagemAnimator.SetBool("Correndo", true); // Se necessário, ajuste conforme a lógica do seu jogo
        }
    }
}
