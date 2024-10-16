using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaCarnivora : MonoBehaviour
{
    private SistemaDeVida sistemaDeVida;

    private void Start()
    {
        sistemaDeVida = FindObjectOfType<SistemaDeVida>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            sistemaDeVida.vida--;
            Debug.Log("O jogador foi atacado pela planta carnívora!");
        }
    }
}
