using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    public LightSwitch lightSwitch;
    private bool playerInZone = false;
    public GameObject botaoInteracao;

    private void Update()
    {
        if(playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            lightSwitch.ToggleLight();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInZone = false;
            botaoInteracao.SetActive(false);
        }
    }
}
