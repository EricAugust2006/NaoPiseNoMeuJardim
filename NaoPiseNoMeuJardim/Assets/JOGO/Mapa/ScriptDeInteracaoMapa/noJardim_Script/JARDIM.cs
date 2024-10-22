using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JARDIM : MonoBehaviour
{
    public GameObject BotaoInciar;
    public GameObject inimigoPrefab;
    public GameObject barreira;

    public bool IniciarJogo = false;
    private bool playerInJardim = false;
    public bool eventoLigado = false;

    private ScriptMae Mae;

    private ScriptPersonagem scriptPersonagem; // Referência ao script do personagem
    private CapsuleCollider2D inimigoCollider;

    private void Start()
    {
        Mae = FindObjectOfType<ScriptMae>();
        scriptPersonagem = FindObjectOfType<ScriptPersonagem>(); // Referência ao script do personagem
        if (inimigoPrefab != null)
        {
            inimigoCollider = inimigoPrefab.GetComponent<CapsuleCollider2D>();
        }
    }

    private void Update() {
        if(playerInJardim == true){
            barreira.SetActive(false);
            IniciarJogo = true;
        };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = true;
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
}
