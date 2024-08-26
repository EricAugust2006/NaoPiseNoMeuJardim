using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrSegundoAndar : MonoBehaviour
{
    public TransicaoDeCenas transicaoDeCenas;

    
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
}
