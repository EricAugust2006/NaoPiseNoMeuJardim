using UnityEngine;
using UnityEngine.SceneManagement;

public class SairDeCasa : MonoBehaviour, IInteractable
{
    private bool Interagido = false;
    public GameObject botaoInterage;

    public TransicaoDeCenas transicaoDeCenas;

    public void Interact()
    {
        Sair();
    }

    private void Sair()
    {
        if (!Interagido)
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 1.");
            Interagido = true;
            //SceneManager.LoadScene(1);
            transicaoDeCenas.CarregarCena("JardimJogo");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            botaoInterage.SetActive(false);
        }
    }
}
