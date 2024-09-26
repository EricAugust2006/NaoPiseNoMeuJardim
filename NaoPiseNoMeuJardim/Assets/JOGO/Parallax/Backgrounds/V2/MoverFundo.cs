using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverFundo : MonoBehaviour
{
    private Vector3 posOriginal; // Posição original do fundo
    private float larguraObjeto; // Armazena o tamanho do objeto (largura)
    public float parallaxEffect; // Efeito de paralaxe (velocidade relativa)
    public float movimentoAutomatico = 1f; // Velocidade automática do fundo
    public GameObject irmao; // O próximo fundo para o loop

    private Transform cameraTransform; // Referência à câmera principal
    private Vector3 lastCameraPos; // Posição anterior da câmera

    void Start()
    {
        // Salva a posição original do fundo
        posOriginal = transform.position;

        // Calcula a largura do objeto automaticamente
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            larguraObjeto = sr.bounds.size.x; // Usa o SpriteRenderer para pegar a largura
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                larguraObjeto = rend.bounds.size.x;
            }
        }

        // Encontra a câmera principal
        cameraTransform = Camera.main.transform;
        // Armazena a posição inicial da câmera
        lastCameraPos = cameraTransform.position;
    }

    private void Update()
    {
        parallaxScene();
    }

    public void parallaxScene()
    {
        // Movimento contínuo para a esquerda
        transform.position += new Vector3(-movimentoAutomatico * Time.deltaTime, 0, 0);

        // Calcula a diferença entre a posição atual da câmera e a última posição armazenada
        float deltaX = cameraTransform.position.x - lastCameraPos.x;

        // Aplica o efeito de paralaxe baseado no movimento da câmera
        transform.position += new Vector3(deltaX * parallaxEffect, 0, 0);

        // Atualiza a última posição da câmera
        lastCameraPos = cameraTransform.position;

        // Reposiciona o fundo quando ele sai da tela à esquerda
        if (transform.position.x < cameraTransform.position.x - larguraObjeto - 10f)
        {
            transform.position = new Vector3(irmao.transform.position.x + larguraObjeto, posOriginal.y, posOriginal.z);
        }
    }
}
