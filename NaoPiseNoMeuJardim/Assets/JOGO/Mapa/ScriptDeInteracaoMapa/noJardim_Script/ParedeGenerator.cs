using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeGenerator : MonoBehaviour
{
    [Header("Configurações da Parede")]
    public GameObject paredePrefab; // Prefab da parede
    public Transform pontoDeSpawn; // Ponto de spawn das paredes (no chão)
    public float velocidade = 5f; // Velocidade das paredes se movendo para a esquerda
    public float limiteDestruicao = -15f; // Limite para destruir paredes fora da tela

    [Header("Configurações de Geração")]
    public float intervaloMinimoGeracao = 2f; // Intervalo mínimo entre gerações de paredes
    public float intervaloMaximoGeracao = 5f; // Intervalo máximo entre gerações de paredes
    [Range(0f, 1f)] public float chanceSpawnParede = 0.5f; // Chance inicial de spawnar uma parede
    public float aumentoChance = 0.1f; // Valor de aumento da chance
    public float maxChance = 1f; // Chance máxima de spawn

    private List<GameObject> paredesAtivas = new List<GameObject>(); // Lista para rastrear as paredes ativas
    private bool jogoIniciado = false; // Controla se o jogo foi iniciado
    private ScriptPersonagem player; // Referência ao personagem

    // Variável para rastrear o tempo decorrido
    private float tempoDecorrido = 0f;
    public float intervaloParaAumentarChance = 30f; // Intervalo para aumentar a chance (30 segundos)

    void Start()
    {
        // Busca pelo componente do personagem
        player = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        // Verifica se o jogo foi iniciado e a condição foi atendida
        if (!jogoIniciado && player.triggouComTagPararCorrida == true)
        {
            jogoIniciado = true;
            StartCoroutine(GerarParedesContinuamente());
        }

        // Se o jogo não foi iniciado, não movimenta nem destrói paredes
        if (!jogoIniciado) return;

        // Atualiza o temporizador
        tempoDecorrido += Time.deltaTime;

        // Aumenta a chance de spawn a cada intervalo de tempo
        if (tempoDecorrido >= intervaloParaAumentarChance)
        {
            AumentarChance();
            tempoDecorrido = 0f; // Reseta o temporizador após aumentar a chance
        }

        // Movimenta as paredes para a esquerda
        MoverParedes();

        // Destroi paredes fora da tela
        DestruirParedes();
    }

    IEnumerator GerarParedesContinuamente()
    {
        while (true)
        {
            // Gera paredes somente se a condição do jogador ainda for verdadeira
            if (player.triggouComTagPararCorrida == true && Random.value < chanceSpawnParede)
            {
                GerarParede();
            }
            // Espera um tempo aleatório antes de gerar a próxima parede
            yield return new WaitForSeconds(Random.Range(intervaloMinimoGeracao, intervaloMaximoGeracao));
        }
    }

    void GerarParede()
    {
        // Calcula a posição de spawn
        float posicaoBaseX = pontoDeSpawn.position.x + 10f; // Distância fixa para spawnar a parede

        GameObject parede = new GameObject("Parede");
        parede.transform.position = new Vector2(posicaoBaseX, pontoDeSpawn.position.y); // Posiciona a parede

        // Cria a parede bloco por bloco (3 blocos de altura)
        for (int i = 0; i < 3; i++)
        {
            Vector2 posicaoBloco = new Vector2(posicaoBaseX, pontoDeSpawn.position.y + i);
            GameObject bloco = Instantiate(paredePrefab, posicaoBloco, Quaternion.identity, parede.transform);
        }

        // Adiciona a parede à lista de paredes ativas
        paredesAtivas.Add(parede);
    }

    void MoverParedes()
    {
        // Movimenta as paredes para a esquerda
        foreach (GameObject parede in paredesAtivas)
        {
            parede.transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }
    }

    void DestruirParedes()
    {
        for (int i = paredesAtivas.Count - 1; i >= 0; i--)
        {
            if (paredesAtivas[i].transform.position.x < limiteDestruicao)
            {
                Destroy(paredesAtivas[i]);
                paredesAtivas.RemoveAt(i);
            }
        }
    }

    // Função para aumentar a chance de spawn
    private void AumentarChance()
    {
        chanceSpawnParede += aumentoChance;
        chanceSpawnParede = Mathf.Clamp(chanceSpawnParede, 0f, maxChance);
        Debug.Log($"Chance de spawn de parede aumentada para: {chanceSpawnParede}");
    }
}
