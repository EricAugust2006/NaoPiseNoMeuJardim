using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaSubirAndar : MonoBehaviour, IInteractable
{
    private bool Interagido = false;

    public void Interact()
    {
        SubirAndar();
    }

    private void SubirAndar()
    {
        if (!Interagido)
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 1.");
            Interagido = true;
            SceneManager.LoadScene(1);
        }
    }
}
