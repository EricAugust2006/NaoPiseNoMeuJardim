using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    [Header("Booleanos")]
    private bool playerInZone = false;
    public bool eventoLigado = false;

    [Header("GameObject e Script")]
    public LightSwitch lightSwitch;
    public GameObject botaoInteracao;
    
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