using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transicaoTeste : MonoBehaviour
{
   public Animator transicaoTelaAnimator; // Referência ao Animator
    public string nomeDaCena; // Nome da cena para carregar

    public void IniciarTransicao()
    {
        StartCoroutine(CarregarCena());
    }

    private IEnumerator CarregarCena()
    {
        // Iniciar a animação de fechamento
        transicaoTelaAnimator.SetTrigger("Fechar");

        // Aguarde a animação de fechar ser concluída
        yield return new WaitForSeconds(5.0f); // Ajuste conforme o tempo da sua animação

        // Carregar a nova cena
        SceneManager.LoadScene(nomeDaCena);

        // Aguarde a nova cena carregar
        yield return null;

        // Iniciar a animação de abertura
        transicaoTelaAnimator.SetTrigger("Abrir");
    }
}
