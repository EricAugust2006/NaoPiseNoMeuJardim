using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InicioDoJogo : MonoBehaviour
{
// Dialogue script variables
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

    // Victory script variables
    public Image victoryPanel;
    public float fadeSpeed = 0.5f;

    void Start()
    {
        dialoguePanel.SetActive(false);
        victoryPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak)
        {
            if (!startDialogue)
            {
                // Disable jump and running animation
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

            // Trigger victory event after dialogue completion
            StartCoroutine(ShowVictory());
        }
    }

    public void StartDialogue()
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

    // Method to disable animations
    void DesativarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Desativando animações");
            personagemAnimator.SetBool("Correndo", false);
            personagemAnimator.SetBool("Caindo", false);
            personagemAnimator.SetFloat("Velocidade", 0f);
        }
    }

    // Method to restore animations
    void RestaurarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Restaurando animações");
            personagemAnimator.SetBool("Correndo", true);
        }
    }

    IEnumerator ShowVictory()
    {
        victoryPanel.gameObject.SetActive(true);
        Color color = victoryPanel.color;

        while (color.a < 1f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            victoryPanel.color = color;
            yield return null;
        }
    }
}
