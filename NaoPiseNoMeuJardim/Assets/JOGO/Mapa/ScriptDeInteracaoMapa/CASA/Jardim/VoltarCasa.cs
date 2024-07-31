using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarCasa : MonoBehaviour, IInteractable
{
    private bool Interagido = false;
    public GameObject botaoInterage;


    public void Interact()
    {
        Voltar();
    }

    private void Voltar()
    {
        if (!Interagido)
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 0.");
            SceneManager.LoadScene(0);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(false);
        }
    }
}

