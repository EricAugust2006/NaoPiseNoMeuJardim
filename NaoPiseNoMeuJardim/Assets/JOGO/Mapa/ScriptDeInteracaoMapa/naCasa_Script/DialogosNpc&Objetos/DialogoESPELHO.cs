using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoESPELHO : MonoBehaviour
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

    public bool Interagido = false;
    public GameObject botaoInteracao;
    public bool tanoDialogoEspelho = false;

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
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak)
        {
            tanoDialogoEspelho = true;
            if (!startDialogue)
            {
                // Desativa pulo e anima��o de corrida
                personagemScript.speed = 0f;
                personagemScript.DesativarAnimacoes();
                StartDialogue();
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue();
            }

            if (!Interagido)
            {
                personagemScript.EmpurrarEspelho();
                Interagido = true;
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
            personagemScript.RestaurarAnimacoes();
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
            readyToSpeak = true;
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
            botaoInteracao.SetActive(false);
        }
    }
}
