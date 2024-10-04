using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GerenciaInicio : MonoBehaviour
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

    public float fadeSpeed = 0.5f;

    public Image blackScreenPanel;

    void Start()
    {
        dialoguePanel.SetActive(false);
        // victoryPanel.gameObject.SetActive(false);

        // Start with black screen active
        blackScreenPanel.gameObject.SetActive(true);
        Color color = blackScreenPanel.color;
        color.a = 1f;
        blackScreenPanel.color = color;

        // Automatically start the dialogue when the scene begins
        StartCoroutine(BeginScene());
    }

    IEnumerator BeginScene()
    {
        // Start the dialogue automatically
        yield return new WaitForSeconds(3.5f); // Wait for 1 second before starting dialogue
        StartDialogue();
    }

    void Update()
    {
        // Dialogue progression happens automatically without player input
        if (dialogueText.text == dialogueNpc[dialogueIndex])
        {
            NextDialogue();
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

            // Trigger screen fade out after dialogue ends
            StartCoroutine(FadeOutBlackScreen());

            // Enable animations after dialogue
            //RestaurarAnimacoes();
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
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Screen fade out to reveal the scene after dialogue
    IEnumerator FadeOutBlackScreen()
    {
        Color color = blackScreenPanel.color;

        // Gradually fade out the black screen
        while (color.a > 0f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            blackScreenPanel.color = color;
            yield return null;
        }
        // Disable black screen panel after fade out
        blackScreenPanel.gameObject.SetActive(false);
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
}
