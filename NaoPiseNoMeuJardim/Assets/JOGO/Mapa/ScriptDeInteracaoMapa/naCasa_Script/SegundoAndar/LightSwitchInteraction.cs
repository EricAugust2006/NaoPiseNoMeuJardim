using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    public LightSwitch lightSwitch;
    private bool playerInZone = false;
    public GameObject botaoInteracao;
    public bool eventoLigado = false;
    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
        {
            lightSwitch.ToggleLight();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = true;
            playerInZone = true;
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            playerInZone = false;
            botaoInteracao.SetActive(false);
        }
    }
}