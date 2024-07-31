using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntrarCozinha : MonoBehaviour, IInteractable
{
    private bool Interagido = false;
    public GameObject botaoInterage;

    public void Interact()
    {
        EntraCozinha();
    }

    private void EntraCozinha()
    {
        if (!Interagido)
        {
            Debug.Log("Pelo visto a cozinha não está disponível para você. :(");
            Interagido = true;
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
