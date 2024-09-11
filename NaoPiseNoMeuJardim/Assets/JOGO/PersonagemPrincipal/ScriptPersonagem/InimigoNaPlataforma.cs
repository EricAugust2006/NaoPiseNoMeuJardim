using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class InimigoNaPlataforma : MonoBehaviour
{
    public GameObject inimigoPrefab; // Prefab do inimigo
    public Tilemap plataformaTilemap; // O Tilemap das plataformas
    public LayerMask layerPlataforma; // Layer das plataformas
    public float tempoParaGerarInimigo = 5f; // Tempo necess�rio para gerar o inimigo

    private float tempoEmPlataforma = 0f;
    private bool emPlataforma = false;
    private Vector3 plataformaAtualPosicao;

    private void Update()
    {
        if (emPlataforma)
        {
            // Incrementa o tempo que o player est� sobre a plataforma
            tempoEmPlataforma += Time.deltaTime;

            // Se o tempo for maior ou igual ao necess�rio, gera o inimigo
            if (tempoEmPlataforma >= tempoParaGerarInimigo)
            {
                GerarInimigo();
                tempoEmPlataforma = 0f; // Reinicia o tempo
            }
        }
        else
        {
            tempoEmPlataforma = 0f; // Reseta o tempo quando sai da plataforma
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            emPlataforma = true;
            plataformaAtualPosicao = CalcularPosicaoPlataforma(transform.position); // Pega a posi��o da plataforma
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            emPlataforma = false;
        }
    }

    private Vector3 CalcularPosicaoPlataforma(Vector3 posicaoPlayer)
    {
        // Converte a posi��o do player para a c�lula da plataforma no Tilemap
        Vector3Int celulaPlataforma = plataformaTilemap.WorldToCell(posicaoPlayer);
        Vector3 posicaoCentroPlataforma = plataformaTilemap.GetCellCenterWorld(celulaPlataforma); // Centro da c�lula
        return posicaoCentroPlataforma;
    }

    private void GerarInimigo()
    {
        // Gera o inimigo na posi��o da plataforma onde o player est�
        Instantiate(inimigoPrefab, plataformaAtualPosicao, Quaternion.identity);
        Debug.Log("Inimigo Gerado!");
    }
}
