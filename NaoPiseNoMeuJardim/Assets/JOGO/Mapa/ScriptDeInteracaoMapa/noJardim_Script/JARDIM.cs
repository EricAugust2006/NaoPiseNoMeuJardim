using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JARDIM : MonoBehaviour
{
    public bool IniciarJogo = false;
    public GameObject BotaoInciar;
    public GameObject inimigoPrefab; // Referência ao prefab do inimigo
    private ScriptMae Mae;
    private bool playerInJardim = false;
    private CapsuleCollider2D inimigoCollider; // Referência ao CapsuleCollider2D do inimigo

    private void Start()
    {
        Mae = FindObjectOfType<ScriptMae>();
        if (inimigoPrefab != null)
        {
            inimigoCollider = inimigoPrefab.GetComponent<CapsuleCollider2D>();
        }
    }

    public void Interact()
    {
        Conversar();
    }

    private void Conversar()
    {
        if (!IniciarJogo)
        {
            Debug.Log("PISOU NO JARDIM");
            IniciarJogo = true;

            if (IniciarJogo)
            {
                Mae.PerseguirFilho();
                Mae.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;  // Define o Rigidbody da mãe como Dynamic
                if (inimigoCollider != null)
                {
                    inimigoCollider.enabled = true; // Habilita o CapsuleCollider do inimigo
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Você pisou no jardim");
            playerInJardim = true;
            BotaoInciar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInJardim = false;
            BotaoInciar.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInJardim && Input.GetKeyDown(KeyCode.E))
        {
            Mae.PerseguirFilho();
            Mae.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;  // Define o Rigidbody da mãe como Dynamic
            if (inimigoCollider != null)
            {
                inimigoCollider.enabled = true; // Habilita o CapsuleCollider do inimigo
            }
        }
    }
}
