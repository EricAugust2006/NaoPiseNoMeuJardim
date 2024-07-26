using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaSubirAndar : MonoBehaviour, IInteractable
{
    private bool Interagido = false;
    public GameObject botaoInterage;

    public void Interact()
    {
        SubirAndar();
    }

    private void SubirAndar()
    {
        if (!Interagido)
        {
            Debug.Log("Interagindo com a porta. Entrando no segundo andar");
            Interagido = true;
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