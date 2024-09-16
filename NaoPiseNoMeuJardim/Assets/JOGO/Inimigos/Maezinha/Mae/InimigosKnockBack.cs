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

            // Adicionar mensagens de debug para verificar as posi��es
            Debug.Log("Posi��o do jogador: " + collision.transform.position.x);
            Debug.Log("Posi��o do inimigo: " + transform.position.x);

            // Verifica a posi��o do jogador em rela��o ao inimigo para definir a dire��o do knockback
            // Verifica a posi��o do jogador em rela��o ao inimigo
            if (collision.transform.position.x < transform.position.x)
            {
                // Jogador est� � esquerda do inimigo, ent�o knockback para a direita
                player.isKnockRight = true;
                Debug.Log("Jogador � esquerda do inimigo, knockback para a direita");
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                // Jogador est� � direita do inimigo, ent�o knockback para a esquerda
                player.isKnockRight = false;
                Debug.Log("Jogador � direita do inimigo, knockback para a esquerda");
            }


            // Aplica o dano ao jogador
            sistemaDeVida.vida--;
        }
    }
}
