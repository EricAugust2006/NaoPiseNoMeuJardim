using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMovel : MonoBehaviour
{
    public float velocidade = 5f; // Velocidade do inimigo
    public float tempoDeVida = 6f; // Tempo at� o inimigo ser destru�do

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
        Destroy(gameObject); // Destr�i o inimigo ap�s o tempo especificado
    }
}
