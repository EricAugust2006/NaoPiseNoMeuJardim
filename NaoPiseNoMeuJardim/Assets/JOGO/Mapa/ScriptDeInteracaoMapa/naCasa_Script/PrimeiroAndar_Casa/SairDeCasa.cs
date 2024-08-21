using UnityEngine;
using UnityEngine.SceneManagement;

public class SairDeCasa : MonoBehaviour
{
    [Header("Script")]
    public TransicaoDeCenas transicaoDeCenas;

    [Header("Booleanos")]
    public bool eventoLigado = false;
    private bool Interagido = false;

    [Header("GameObjects")]
    public GameObject botaoInterage;

    public void Update()
    {
        Sair();
    }

    private void Sair()
    {
        if (Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
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
