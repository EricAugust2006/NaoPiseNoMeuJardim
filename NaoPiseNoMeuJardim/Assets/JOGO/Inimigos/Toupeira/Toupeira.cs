using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toupeira : MonoBehaviour
{
    private SistemaDeVida sistemaDeVida;
    private ScriptPersonagem player;
    private SpriteRenderer playerSpriteRenderer;
    private void Start()

    {
        player = FindObjectOfType<ScriptPersonagem>();
        sistemaDeVida = FindObjectOfType<SistemaDeVida>();
        if (player != null)
        {
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // player.kbCount = player.kBTime; // Inicia o contador de knockback

            // // Adicionar mensagens de debug para verificar as posi��es
            // Debug.Log("Posi��o do jogador: " + other.transform.position.x);
            // Debug.Log("Posi��o do inimigo: " + transform.position.x);

            // // Verifica a posi��o do jogador em rela��o ao inimigo para definir a dire��o do knockback
            // // Verifica a posi��o do jogador em rela��o ao inimigo
            // if (other.transform.position.x < transform.position.x)
            // {
            //     // Jogador est� � esquerda do inimigo, ent�o knockback para a direita
            //     player.isKnockRight = true;
            //     Debug.Log("Jogador � esquerda do inimigo, knockback para a direita");
            // }
            // else if (other.transform.position.x > transform.position.x)
            // {
            //     // Jogador est� � direita do inimigo, ent�o knockback para a esquerda
            //     player.isKnockRight = false;
            //     Debug.Log("Jogador � direita do inimigo, knockback para a esquerda");
            // }

            sistemaDeVida.vida--;
            // Aplica o dano ao jogador
        }

        if (other.gameObject.tag == "PlayerAtk")
        {
            Debug.Log("o bixo colidiu cmg");
            player.InimigoEmpurrar();
            Destroy(gameObject);
        }
    }
}
