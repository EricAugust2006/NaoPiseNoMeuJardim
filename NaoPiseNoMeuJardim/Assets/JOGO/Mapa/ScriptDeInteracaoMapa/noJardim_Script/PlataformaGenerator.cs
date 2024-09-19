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
    private float[] posicoesY = new float[] { 2f, 12f, 18f, 24f }; // As quatro linhas horizontais
    private float proximaPosicaoX = 0f; // A posição X onde a próxima plataforma será gerada

    // Define cores para cada linha
    private Color[] coresDasLinhas = new Color[] {
        Color.red,    // Linha 1 (Y = -3f)
        Color.green,  // Linha 2 (Y = -1f)
        Color.blue,   // Linha 3 (Y = 1f)
        Color.yellow  // Linha 4 (Y = 3f)
    };

    void Start()
    {
        // Inicia a geração contínua de plataformas
        StartCoroutine(GerarPlataformasContinuamente());
    }

    void Update()
    {
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
        // Escolhe um tamanho aleatório entre 4 e 8 blocos
        int tamanhoPlataforma = Random.Range(4, 9);

        // Escolhe uma posição aleatória Y (uma das 4 linhas)
        int indiceLinha = Random.Range(0, posicoesY.Length);
        float posicaoY = posicoesY[indiceLinha];
        Color corDaLinha = coresDasLinhas[indiceLinha]; // Cor associada à linha

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
        // Movimenta cada plataforma da lista para a esquerda
        foreach (GameObject plataforma in plataformasAtivas)
        {
            plataforma.transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }
    }

    void DestruirPlataformas()
    {
        // Verifica as plataformas que saíram do limite e as destrói
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
