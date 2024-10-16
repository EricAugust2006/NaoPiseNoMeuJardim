using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toupeira : MonoBehaviour
{
    private SistemaDeVida sistemaDeVida;
    private ScriptPersonagem player;
    private Animator animator;

    // Chance de dar uma vida ao jogador (0 a 1)
    [Range(0f, 1f)]
    public float chanceDarVida = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<ScriptPersonagem>();
        sistemaDeVida = FindObjectOfType<SistemaDeVida>();

        animator.SetTrigger("Spawn");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            sistemaDeVida.vida--;
        }

        if (other.gameObject.tag == "PlayerAtk")
        {
            animator.SetTrigger("Despawn");
            Debug.Log("A toupeira foi atacada!");

            // Verifica se o jogador pode receber uma vida extra
            if (Random.value <= chanceDarVida && sistemaDeVida.vida < sistemaDeVida.vidaMaxima)
            {
                sistemaDeVida.vida++; // Aumenta a vida do jogador
                Debug.Log("A toupeira deu uma vida ao jogador!");
            }

            player.InimigoEmpurrar();
            Destroy(gameObject);
        }
    }
}
