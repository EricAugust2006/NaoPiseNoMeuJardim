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
    public float chanceSpawnParede = 0.5f; // Chance de spawnar uma parede

    private List<GameObject> paredesAtivas = new List<GameObject>(); // Lista para rastrear as paredes ativas
    private bool jogoIniciado = false;

    private ScriptPersonagem player;

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        // Verifica se o jogo foi iniciado
        if (!jogoIniciado)
        {
            jogoIniciado = true;
            StartCoroutine(GerarParedesContinuamente());
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
            if (Random.value < chanceSpawnParede)
            {
                GerarParede();
            }
            // Espera um tempo aleatório antes de gerar a próxima parede
            yield return new WaitForSeconds(Random.Range(intervaloMinimoGeracao, intervaloMaximoGeracao));
        }
    }

    void GerarParede()
    {
        if (player.triggouComTagPararCorrida == true)
        {

            // Calcula a posição de spawn baseada em uma distância fixa da posição do ponto de spawn
            float posicaoBaseX = pontoDeSpawn.position.x + 10f; // Distância fixa para spawnar a parede

            GameObject parede = new GameObject("Parede");
            parede.transform.position = new Vector2(posicaoBaseX, pontoDeSpawn.position.y); // Posiciona a parede no chão

            // Cria a parede bloco por bloco (3 blocos de altura)
            for (int i = 0; i < 3; i++)
            {
                Vector2 posicaoBloco = new Vector2(posicaoBaseX, pontoDeSpawn.position.y + i);
                GameObject bloco = Instantiate(paredePrefab, posicaoBloco, Quaternion.identity, parede.transform);
            }

            // Adiciona a parede à lista de paredes ativas
            paredesAtivas.Add(parede);
        }
    }

    void MoverParedes()
    {
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

    // Método para lidar com a colisão do jogador com a parede
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Exemplo de ação quando o jogador colide com a parede
            ScriptPersonagem personagem = collision.GetComponent<ScriptPersonagem>();

            if (personagem != null)
            {
                // O jogador perde vida
                // personagem.PerdeVida(1);

                // O jogador fica intangível por um tempo
                StartCoroutine(FicarIntangivel(personagem));
            }
        }
    }

    IEnumerator FicarIntangivel(ScriptPersonagem personagem)
    {
        // personagem.ficarIntangivel = true; // Supondo que o jogador tenha uma flag de invulnerabilidade

        // Tempo de intangibilidade (ex: 2 segundos)
        yield return new WaitForSeconds(2f);

        // personagem.ficarIntangivel = false;
    }
}