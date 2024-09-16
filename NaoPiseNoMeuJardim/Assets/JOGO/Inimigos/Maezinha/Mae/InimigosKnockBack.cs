using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigosKnockBack : MonoBehaviour
{
    private SistemaDeVida sistemaDeVida;
    private ScriptPersonagem player;
    private SpriteRenderer playerSpriteRenderer;
    private void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        sistemaDeVida = FindObjectOfType<SistemaDeVida>();
        if(player != null)
        {
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.kbCount = player.kBTime; // Inicia o contador de knockback

            // Adicionar mensagens de debug para verificar as posições
            Debug.Log("Posição do jogador: " + collision.transform.position.x);
            Debug.Log("Posição do inimigo: " + transform.position.x);

            // Verifica a posição do jogador em relação ao inimigo para definir a direção do knockback
            // Verifica a posição do jogador em relação ao inimigo
            if (collision.transform.position.x < transform.position.x)
            {
                // Jogador está à esquerda do inimigo, então knockback para a direita
                player.isKnockRight = true;
                Debug.Log("Jogador à esquerda do inimigo, knockback para a direita");
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                // Jogador está à direita do inimigo, então knockback para a esquerda
                player.isKnockRight = false;
                Debug.Log("Jogador à direita do inimigo, knockback para a esquerda");
            }


            // Aplica o dano ao jogador
            sistemaDeVida.vida--;
        }
    }


}
