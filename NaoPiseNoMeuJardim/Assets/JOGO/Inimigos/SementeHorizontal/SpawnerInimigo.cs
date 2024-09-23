using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInimigo : MonoBehaviour
{
    public GameObject inimigoPrefab; // Prefab do inimigo
    public float intervaloSpawn = 2f; // Tempo entre os spawns

    void Start()
    {
        StartCoroutine(SpawnarInimigos());
    }

    IEnumerator SpawnarInimigos()
    {
        while (true)
        {
            SpawnarInimigo();
            yield return new WaitForSeconds(intervaloSpawn);
        }
    }

    void SpawnarInimigo()
    {
        // Posição do spawn (ajuste conforme necessário)
        Vector2 posicaoSpawn = transform.position;
        posicaoSpawn.x -= 1f; // Ajuste a posição para spawnar à esquerda do spawner
        Instantiate(inimigoPrefab, posicaoSpawn, Quaternion.identity);
    }
}
