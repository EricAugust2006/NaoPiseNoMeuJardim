//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlataformaDestruidora : MonoBehaviour
//{
//    public float velocidade = 5f;
//    public Transform player; // Vari�vel para o player

//    void Start()
//    {
//        if (player == null)
//        {
//            player = GameObject.FindGameObjectWithTag("Player").transform;
//        }
//    }

//    void Update()
//    {
//        transform.Translate(Vector3.left * velocidade * Time.deltaTime);

//        if (player != null)
//        {
//            // Acesse a posi��o do player aqui
//            Vector3 playerPosition = player.position;
//            // Se a plataforma estiver muito atr�s do jogador, destrua-a

//            // L�gica para destruir a plataforma ou outro comportamento
//            if (transform.position.x < player.position.x - 20f)
//            {
//                Destroy(gameObject);
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Player n�o atribu�do ao PlataformaDestruidora");
//        }
//    }
//}
