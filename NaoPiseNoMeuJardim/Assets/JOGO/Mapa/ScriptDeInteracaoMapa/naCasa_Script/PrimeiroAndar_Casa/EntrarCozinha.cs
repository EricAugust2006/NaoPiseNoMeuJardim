using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntrarCozinha : MonoBehaviour
{
    public bool eventoLigado = false;
    private bool Interagido = false;
    public GameObject botaoInterage;

    public void Update()
    {
        EntraCozinha();
    }

    private void EntraCozinha()
    {
        if (Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
        {
            Debug.Log("(ENTROU EVENTO)Pelo visto a cozinha n�o est� dispon�vel para voc�. :(");
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
