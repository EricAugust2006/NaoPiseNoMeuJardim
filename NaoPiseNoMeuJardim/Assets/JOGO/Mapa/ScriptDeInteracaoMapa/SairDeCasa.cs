using UnityEngine;
using UnityEngine.SceneManagement;

public class SairDeCasa : MonoBehaviour, IInteractable
{
    private bool Interagido = false;

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
            SceneManager.LoadScene(1);
        }
    }
}
