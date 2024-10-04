using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGenerator : MonoBehaviour
{
    [Header("Configurações da Plataforma")]
    public GameObject blocoPrefab; // Prefab do bloco de plataforma
    public List<Transform> pontosDeSpawn; // Lista de pontos de spawn das plataformas
    public float velocidade = 5f; // Velocidade das plataformas se movendo para a esquerda
    public float limiteDestruicao = -15f; // Limite para destruir plataformas fora da tela
    public Color[] coresDasLinhas; // Cores das linhas de plataformas (configurável)

    [Header("Configurações de Geração")]
    public float intervaloMinimoGeracao = 2f; // Intervalo mínimo entre gerações de plataformas
    public float intervaloMaximoGeracao = 4f; // Intervalo máximo entre gerações de plataformas
    public int tamanhoMinimoPlataforma = 4; // Tamanho mínimo da plataforma
    public int tamanhoMaximoPlataforma = 8; // Tamanho máximo da plataforma
    public float espacamentoMinimoPlataforma = 4f; // Espaçamento mínimo entre plataformas
    public float espacamentoMaximoPlataforma = 7f; // Espaçamento máximo entre plataformas

    [Header("Configurações de Inimigos")]
    public GameObject inimigoPrefab; // Prefab do inimigo
    public float chanceSpawnInimigo = 0.3f; // Chance de spawnar inimigo por plataforma

    private List<GameObject> plataformasAtivas = new List<GameObject>(); // Lista para rastrear as plataformas ativas
    private JARDIM jardim;
    private bool jogoIniciado = false;

    // Posiciona a próxima plataforma sempre a uma distância fixa do ponto de spawn
    private float distanciaFixadaX = 10f; // Distância fixa da câmera/jogador para o spawn das plataformas

    private ScriptPersonagem player;

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        jardim = FindObjectOfType<JARDIM>();
    }

    void Update()
    {
        // Verifica se o jogo foi iniciado e o trigger foi ativado
        if (jardim != null && jardim.IniciarJogo && player.triggouComTagPararCorrida && !jogoIniciado)
        {
            jogoIniciado = true;
            StartCoroutine(GerarPlataformasContinuamente());
        }

        // Se o jogo não foi iniciado, não faz nada
        if (!jogoIniciado)
        {
            return;
        }

        // Movimenta as plataformas para a esquerda
        MoverPlataformas();

        // Destroi plataformas fora da tela
        DestruirPlataformas();
    }

    IEnumerator GerarPlataformasContinuamente()
    {
        while (true)
        {
            GerarPlataforma();
            // Espera um tempo aleatório antes de gerar a próxima plataforma
            yield return new WaitForSeconds(
                Random.Range(intervaloMinimoGeracao, intervaloMaximoGeracao)
            );
        }
    }

    void GerarPlataforma()
    {
        if (pontosDeSpawn.Count == 0)
            return; // Se não houver pontos de spawn, sair da função

        // Escolhe um ponto de spawn aleatório da lista
        Transform pontoDeSpawn = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Count)];
        int tamanhoPlataforma = Random.Range(tamanhoMinimoPlataforma, tamanhoMaximoPlataforma + 1); // Tamanho aleatório da plataforma
        Color corDaLinha = coresDasLinhas[Random.Range(0, coresDasLinhas.Length)];

        // Calcula a posição de spawn baseada em uma distância fixa da posição do ponto de spawn
        float posicaoBaseX = pontoDeSpawn.position.x + distanciaFixadaX;

        GameObject plataforma = new GameObject("Plataforma");
        plataforma.transform.position = pontoDeSpawn.position; // Posiciona a nova plataforma

        // Cria a plataforma bloco por bloco no ponto de spawn
        for (int i = 0; i < tamanhoPlataforma; i++)
        {
            Vector2 posicaoBloco = new Vector2(posicaoBaseX + i, pontoDeSpawn.position.y);
            GameObject bloco = Instantiate(
                blocoPrefab,
                posicaoBloco,
                Quaternion.identity,
                plataforma.transform
            );

            // // Muda a cor do bloco de acordo com a linha
            // SpriteRenderer sr = bloco.GetComponent<SpriteRenderer>();
            // if (sr != null)
            // {
            //     sr.color = corDaLinha;
            // }
        }

        // Decide se vai spawnar um inimigo para essa plataforma
        if (Random.value < chanceSpawnInimigo)
        {
            SpawnarInimigo(plataforma);
        }

        plataformasAtivas.Add(plataforma); // Adiciona a plataforma inteira à lista de plataformas ativas
    }

    void SpawnarInimigo(GameObject plataforma)
    {
        // Obtém o número total de blocos na plataforma
        int numeroDeBlocos = plataforma.transform.childCount;

        // Garante que haja pelo menos um bloco para spawnar o inimigo
        if (numeroDeBlocos == 0)
            return;

        // Escolhe um bloco aleatório da plataforma
        int indiceBlocoAleatorio = Random.Range(0, numeroDeBlocos);
        Transform blocoAleatorio = plataforma.transform.GetChild(indiceBlocoAleatorio);

        // Ajusta a posição para spawnar o inimigo no topo do bloco aleatório
        Vector2 posicaoInimigo = new Vector2(
            blocoAleatorio.position.x,
            blocoAleatorio.position.y + 0.5f
        ); // Ajuste de altura
        GameObject inimigo = Instantiate(inimigoPrefab, posicaoInimigo, Quaternion.identity);

        // Define a plataforma como o pai do inimigo para que ele se mova junto com a plataforma
        inimigo.transform.parent = plataforma.transform;
    }

    void MoverPlataformas()
    {
        foreach (GameObject plataforma in plataformasAtivas)
        {
            plataforma.transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }
    }

    void DestruirPlataformas()
    {
        for (int i = plataformasAtivas.Count - 1; i >= 0; i--)
        {
            if (plataformasAtivas[i].transform.position.x < limiteDestruicao)
            {
                Destroy(plataformasAtivas[i]);
                plataformasAtivas.RemoveAt(i);
            }
        }
    }
}
