using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverFundo : MonoBehaviour
{
    private Vector3 posOriginal;
    private float larguraObjeto;
    public float parallaxEffect;
    public float movimentoAutomatico = 1f;
    public GameObject irmao;

    private Transform cameraTransform;
    private Vector3 lastCameraPos;

    void Start()
    {
        posOriginal = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            larguraObjeto = sr.bounds.size.x;
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                larguraObjeto = rend.bounds.size.x;
            }
        }
        cameraTransform = Camera.main.transform;

        lastCameraPos = cameraTransform.position;
    }

    private void Update()
    {
        parallaxScene();
    }

    public void parallaxScene()
    {
        transform.position += new Vector3(-movimentoAutomatico * Time.deltaTime, 0, 0);

        float deltaX = cameraTransform.position.x - lastCameraPos.x;

        transform.position += new Vector3(deltaX * parallaxEffect, 0, 0);

        lastCameraPos = cameraTransform.position;

        if (transform.position.x < cameraTransform.position.x - larguraObjeto)
        {
            float deslocamentoSobreposicao = 0.5f;
            transform.position = new Vector3(irmao.transform.position.x + larguraObjeto - deslocamentoSobreposicao, posOriginal.y, posOriginal.z);
        }
    }
}