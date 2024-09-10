using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JARDIM : MonoBehaviour
{
    public bool IniciarJogo = false;
    public GameObject BotaoInciar;
    public GameObject inimigoPrefab; // Refer�ncia ao prefab do inimigo
    private ScriptMae Mae;
    private bool playerInJardim = false;
    private CapsuleCollider2D inimigoCollider; // Refer�ncia ao CapsuleCollider2D do inimigo
    public bool eventoLigado = false;

    public GameObject barreira;

    private void Start()
    {
        Mae = FindObjectOfType<ScriptMae>();
        // Mae.GetComponent<Collider2D>().enabled = false;
        if (inimigoPrefab != null)
        {
            inimigoCollider = inimigoPrefab.GetComponent<CapsuleCollider2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = true;
            Debug.Log("Voc� pisou no jardim");
            playerInJardim = true;
            BotaoInciar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = false;
            playerInJardim = false;
            BotaoInciar.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInJardim && Input.GetKeyDown(KeyCode.E))
        {
            barreira.SetActive(false);
            IniciarJogo = true;
            Mae.PerseguirFilho();
            // Mae.GetComponent<Collider2D>().enabled = true;
            Mae.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;  // Define o Rigidbody da m�e como Dynamic
            if (inimigoCollider != null)
            {
                inimigoCollider.enabled = true; // Habilita o CapsuleCollider do inimigo
            }
        }
    }
}
