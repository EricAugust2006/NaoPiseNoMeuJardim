using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMovel : MonoBehaviour
{
    public float velocidade = 5f; // Velocidade do inimigo
    public float tempoDeVida = 6f; // Tempo até o inimigo ser destruído

    void Start()
    {
        StartCoroutine(DestruirInimigo());
    }

    void Update()
    {
        // Move o inimigo para a esquerda
        transform.Translate(Vector2.left * velocidade * Time.deltaTime);
    }

    IEnumerator DestruirInimigo()
    {
        yield return new WaitForSeconds(tempoDeVida);
        Destroy(gameObject); // Destrói o inimigo após o tempo especificado
    }
}
