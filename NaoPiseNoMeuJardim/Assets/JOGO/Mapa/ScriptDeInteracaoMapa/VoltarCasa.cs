using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarCasa : MonoBehaviour, IInteractable
{
    private bool Interagido = false;

    public void Interact()
    {
        Voltar();
    }

    private void Voltar()
    {
        if (!Interagido)
        {
            Debug.Log("Interagindo com a porta. Teleportando para a cena 0.");
            SceneManager.LoadScene(0);
        }
    }
}
