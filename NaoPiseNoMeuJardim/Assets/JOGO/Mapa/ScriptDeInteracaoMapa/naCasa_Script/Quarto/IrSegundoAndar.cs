using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrSegundoAndar : MonoBehaviour
{
    public TransicaoDeCenas transicaoDeCenas;
    public GameObject botaoInterage;
    public GameObject botaoE;

    public bool eventoLigado = false;

    void Start()
    {
        botaoInterage.SetActive(false);
        botaoE.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
        {
            transicaoDeCenas.CarregarCena("SegundoAndar");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(true);
            botaoE.SetActive(true);
            eventoLigado = true;
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(false);
            botaoE.SetActive(false);
            eventoLigado = false;
            botaoInterage.SetActive(false);
        }
    }

}
