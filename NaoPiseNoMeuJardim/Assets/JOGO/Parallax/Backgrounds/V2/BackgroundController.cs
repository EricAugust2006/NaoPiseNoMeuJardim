using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    private float length;
    public GameObject cam;
    public float parallaxEffect;

    [Header("Configurações de Sobreposição")]
    public float sobreposicao = 1f; // Valor de sobreposição do background

    void Start()
    {
        // Posição inicial do background e comprimento do sprite
        startPos = transform.position.x;    
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calcula a distância de movimento da câmera
        float distance = cam.transform.position.x * parallaxEffect; 

        // Atualiza a posição do background
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Reposiciona o background para criar um efeito de sobreposição
        if (cam.transform.position.x > startPos + length - sobreposicao) // Ajuste baseado na variável de sobreposição
        {
            startPos += length; // Move a posição inicial para a direita
        }
        else if (cam.transform.position.x < startPos - sobreposicao) // Ajuste baseado na variável de sobreposição
        {
            startPos -= length; // Move a posição inicial para a esquerda
        }
    }
}
