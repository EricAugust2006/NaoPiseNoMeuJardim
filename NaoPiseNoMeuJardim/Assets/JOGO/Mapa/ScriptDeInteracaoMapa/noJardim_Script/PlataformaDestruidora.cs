//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlataformaDestruidora : MonoBehaviour
//{
//    public float velocidade = 5f;
//    public Transform player; // Variável para o player

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
//            // Acesse a posição do player aqui
//            Vector3 playerPosition = player.position;
//            // Se a plataforma estiver muito atrás do jogador, destrua-a

//            // Lógica para destruir a plataforma ou outro comportamento
//            if (transform.position.x < player.position.x - 20f)
//            {
//                Destroy(gameObject);
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Player não atribuído ao PlataformaDestruidora");
//        }
//    }
//}
