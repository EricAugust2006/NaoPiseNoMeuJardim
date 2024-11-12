using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoMãe : MonoBehaviour
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

    private ScriptPersonagem personagemScript;
    private Animator personagemAnimator;
    private JARDIM jardim;

    public GameObject botaoAvançarDialogo;

    void Start()
    {
        botaoAvançarDialogo.SetActive(false);
        jardim = FindObjectOfType<JARDIM>();
        dialoguePanel.SetActive(false);
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        if (personagemScript != null)
        {
            personagemAnimator = personagemScript.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("ScriptPersonagem não encontrado na cena!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak)
        {
            if (!startDialogue)
            {
                personagemScript.speed = 0f;
                DesativarAnimacoes();
                StartDialogue();
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue();
            }
        }

        if (jardim.IniciarJogo == true)
        {
            GetComponent<Collider2D>().enabled = false;
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
        nameNpc.text = "Mãe";
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

        botaoAvançarDialogo.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = true;
        }
        return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
        }
        return;
    }

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

    void RestaurarAnimacoes()
    {
        if (personagemAnimator != null)
        {
            Debug.Log("Restaurando animações");
            personagemAnimator.SetBool("Correndo", true); 
        }
    }
}
