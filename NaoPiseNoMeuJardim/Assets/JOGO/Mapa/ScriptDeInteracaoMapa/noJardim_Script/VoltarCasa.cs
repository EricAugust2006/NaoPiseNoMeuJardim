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

    public void Update()
    {
        Voltar();
    }

    private void Voltar()
    {
        if (!Interagido && eventoLigado == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 0.");
            //SceneManager.LoadScene(0);
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

