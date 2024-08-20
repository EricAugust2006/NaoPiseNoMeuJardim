using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracaoESPELHO : MonoBehaviour
{
    public bool eventoLigado = false;
    public bool Interagido = false;
    public GameObject botaoInterage;
    private ScriptPersonagem personagem;

    public void Update()
    {
        Espelho();
    }

    private void Start()
    {
        personagem = FindObjectOfType<ScriptPersonagem>(); 
    }

    private void Espelho()
    {
        if (!Interagido && eventoLigado == true)
        {
            Debug.Log("(ENTROU EVENTO)� voc�!");
            personagem.Empurrar();
            Interagido = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = true;
            botaoInterage.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eventoLigado = false;
            botaoInterage.SetActive(false);
        }
    }
}