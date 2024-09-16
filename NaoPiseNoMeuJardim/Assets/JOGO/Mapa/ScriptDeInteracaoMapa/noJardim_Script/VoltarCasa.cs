using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarCasa : MonoBehaviour
{
    public bool eventoLigado = false;
    public bool Interagido = false;
    public GameObject botaoInterage;
    public TransicaoDeCenas transicaoDeCenas;
    private JARDIM jardim;

    private void Start()
    {
        jardim = FindObjectOfType<JARDIM>();
    }

    public void Update()
    {
        Voltar();
    }

    private void Voltar()
    {
        // Verifica se o jogo já foi iniciado, impedindo a interação se for o caso
        if (jardim.IniciarJogo == true)
        {
            return; // Não permite a interação
        }

        // Se o jogador não interagiu ainda, e o evento está ligado, permite a interação
        if (!Interagido && eventoLigado == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 0.");
            transicaoDeCenas.CarregarCena("primeiroAndar");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = true;
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = false;
            botaoInterage.SetActive(false);
        }
    }
}
