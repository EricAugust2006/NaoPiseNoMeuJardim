using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGenerator : MonoBehaviour
{
    public GameObject blocoPrefab; // Prefab do bloco de plataforma
    public float distanciaGeracao = 20f; // Distância entre uma geração e outra
    public float velocidade = 5f; // Velocidade das plataformas se movendo para a esquerda
    public float limiteDestruicao = -15f; // Limite para destruir plataformas fora da tela

    private List<GameObject> plataformasAtivas = new List<GameObject>(); // Lista para rastrear as plataformas ativas
    private float[] posicoesY = new float[] { 2f, 3f }; // As duas linhas horizontais

    private float proximaPosicaoX = 0f; // A posição X onde a próxima plataforma será gerada

    private JARDIM jardim;

    // Define cores para cada linha
    private Color[] coresDasLinhas = new Color[] {
        Color.red,    // Linha 1
        Color.green,  // Linha 2
    };

    private bool jogoIniciado = false;

    void Start()
    {
        jardim = FindObjectOfType<JARDIM>();
    }

    void Update()
    {
        // Verifica se o jogo foi iniciado
        if (jardim != null && jardim.IniciarJogo && !jogoIniciado)
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
            // Espera um tempo antes de gerar a próxima plataforma
            yield return new WaitForSeconds(1f); // Gera uma nova plataforma a cada 1 segundo
        }
    }

    void GerarPlataforma()
    {
        int tamanhoPlataforma = Random.Range(4, 9); // Escolhe um tamanho aleatório entre 4 e 8 blocos
        int indiceLinha = Random.Range(0, posicoesY.Length);
        float posicaoY = posicoesY[indiceLinha];
        Color corDaLinha = coresDasLinhas[indiceLinha];

        // Cria a plataforma bloco por bloco
        for (int i = 0; i < tamanhoPlataforma; i++)
        {
            Vector2 posicaoBloco = new Vector2(proximaPosicaoX + i, posicaoY);
            GameObject bloco = Instantiate(blocoPrefab, posicaoBloco, Quaternion.identity);

            // Muda a cor do bloco de acordo com a linha
            SpriteRenderer sr = bloco.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = corDaLinha;
            }

            plataformasAtivas.Add(bloco); // Adiciona à lista de plataformas ativas
        }

        // Atualiza a posição X para a próxima plataforma
        proximaPosicaoX += tamanhoPlataforma + Random.Range(1, 4); // Espaço entre as plataformas
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
