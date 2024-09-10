using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrSegundoAndar : MonoBehaviour
{
    public TransicaoDeCenas transicaoDeCenas;
    public GameObject botaoInterage;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            transicaoDeCenas.CarregarCena("SegundoAndar");            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(false);
        }
    }

}
