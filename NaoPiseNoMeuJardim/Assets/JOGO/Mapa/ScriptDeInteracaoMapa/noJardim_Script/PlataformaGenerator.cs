using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGenerator : MonoBehaviour
{
    [Header("Configurações da Plataforma")]
    public GameObject blocoPrefab;
    public List<Transform> pontosDeSpawn;
    public float velocidade = 5f;
    public float limiteDestruicao = -15f;
    public Color[] coresDasLinhas;

    [Header("Configurações de Geração")]
    public float intervaloMinimoGeracao = 2f;
    public float intervaloMaximoGeracao = 4f;
    public int tamanhoMinimoPlataforma = 4;
    public int tamanhoMaximoPlataforma = 8;
    public float espacamentoMinimoPlataforma = 4f;
    public float espacamentoMaximoPlataforma = 7f;

    [Header("Configurações de Inimigos")]
    public GameObject toupeiraPrefab; // Prefab da toupeira
    public GameObject plantaCarnivoraPrefab; // Prefab da planta carnívora
    public float chanceSpawnInimigo = 0.3f; // Chance de spawnar um inimigo

    private List<GameObject> plataformasAtivas = new List<GameObject>();
    private JARDIM jardim;
    private bool jogoIniciado = false;

    private float distanciaFixadaX = 10f;
    private ScriptPersonagem player;

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        jardim = FindObjectOfType<JARDIM>();
    }

    void Update()
    {
        if (jardim != null && jardim.IniciarJogo && player.triggouComTagPararCorrida && !jogoIniciado)
        {
            jogoIniciado = true;
            StartCoroutine(GerarPlataformasContinuamente());
        }

        if (!jogoIniciado) return;

        MoverPlataformas();
        DestruirPlataformas();
    }

    IEnumerator GerarPlataformasContinuamente()
    {
        while (true)
        {
            GerarPlataforma();
            yield return new WaitForSeconds(
                Random.Range(intervaloMinimoGeracao, intervaloMaximoGeracao)
            );
        }
    }

    void GerarPlataforma()
    {
        if (pontosDeSpawn.Count == 0)
            return;

        Transform pontoDeSpawn = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Count)];
        int tamanhoPlataforma = Random.Range(tamanhoMinimoPlataforma, tamanhoMaximoPlataforma + 1);
        Color corDaLinha = coresDasLinhas[Random.Range(0, coresDasLinhas.Length)];
        float posicaoBaseX = pontoDeSpawn.position.x + distanciaFixadaX;

        GameObject plataforma = new GameObject("Plataforma");
        plataforma.transform.position = pontoDeSpawn.position;

        for (int i = 0; i < tamanhoPlataforma; i++)
        {
            Vector2 posicaoBloco = new Vector2(posicaoBaseX + i, pontoDeSpawn.position.y);
            Instantiate(blocoPrefab, posicaoBloco, Quaternion.identity, plataforma.transform);
        }

        if (Random.value < chanceSpawnInimigo)
        {
            SpawnarInimigo(plataforma);
        }

        plataformasAtivas.Add(plataforma);
    }

    void SpawnarInimigo(GameObject plataforma)
    {
        int numeroDeBlocos = plataforma.transform.childCount;
        if (numeroDeBlocos == 0) return;

        int indiceBlocoAleatorio = Random.Range(0, numeroDeBlocos);
        Transform blocoAleatorio = plataforma.transform.GetChild(indiceBlocoAleatorio);

        Vector2 posicaoInimigo = new Vector2(
            blocoAleatorio.position.x,
            blocoAleatorio.position.y + 0.5f
        );

        // Define aleatoriamente entre toupeira ou planta carnívora
        GameObject inimigoPrefab = Random.value < 0.5f ? toupeiraPrefab : plantaCarnivoraPrefab;
        GameObject inimigo = Instantiate(inimigoPrefab, posicaoInimigo, Quaternion.identity);

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
