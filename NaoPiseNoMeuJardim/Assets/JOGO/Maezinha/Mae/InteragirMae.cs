using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteragirMae : MonoBehaviour
{
    private bool Interagido = false;
    public GameObject botaoInterage;


    public void Interact()
    {
        Conversar();
    }

    private void Conversar()
    {
        if (!Interagido)
        {
            Debug.Log("O Jardim est� muito bonito, n�o quero voc� perto dele");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
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
