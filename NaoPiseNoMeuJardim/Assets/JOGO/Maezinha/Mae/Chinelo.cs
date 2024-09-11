using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chinelo : MonoBehaviour
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
        if(collision.gameObject.tag == "Player")
        {
            player.kbCount = player.kBTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                player.isKnockRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                player.isKnockRight = false;
            }
            sistemaDeVida.vida--;
            Destroy(this.gameObject);
        }
    }
}
