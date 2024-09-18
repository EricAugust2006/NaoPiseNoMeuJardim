using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrSegundoAndar : MonoBehaviour
{
    public TransicaoDeCenas transicaoDeCenas;
    public GameObject botaoInterage;
    public bool eventoLigado = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && eventoLigado == true){
            transicaoDeCenas.CarregarCena("SegundoAndar");            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            eventoLigado = true;
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            eventoLigado = false;
            botaoInterage.SetActive(false);
        }
    }

}
