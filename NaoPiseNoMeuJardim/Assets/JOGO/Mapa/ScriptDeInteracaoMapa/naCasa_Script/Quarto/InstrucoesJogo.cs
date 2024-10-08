using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Instrucao
{
    public Sprite imagemTecla;   // Imagem da tecla
    [TextArea] public string descricaoAcao; // Descrição da ação
}

public class InstrucoesJogo : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject BotaoInteracao;
    
    // Elementos para as instruções
    public Image telaPreta;
    public GameObject painelInstrucoes;
    public Image imagemTecla;
    public TextMeshProUGUI descricaoAcao;
    public Button botaoProximo;
    public Button botaoAnterior;

    [Header("Configurações")]
    public List<Instrucao> listaInstrucoes; // Lista das instruções
    public float velocidadeTransicao = 0.5f; // Controle de velocidade da transição da tela preta
    private int indiceAtual = 0; // Índice da instrução atual

    [Header("Booleanos")]
    public bool eventoLigado = false;    
    public bool taNoEventoInstrucoes = false;

    private void Start()
    {
        telaPreta.color = new Color(0, 0, 0, 0); // Iniciar com tela transparente
        painelInstrucoes.SetActive(false); // O painel começa invisível
        botaoProximo.onClick.AddListener(MostrarProximaInstrucao); // Associa o botão "Próximo" ao método
        botaoAnterior.onClick.AddListener(MostrarInstrucaoAnterior); // Associa o botão "Anterior" ao método
    }

    private void Update()
    {
        if (eventoLigado && Input.GetKeyDown(KeyCode.F1)) // Tecla F1 para abrir o painel de instruções
        {
            taNoEventoInstrucoes = true;
            StartCoroutine(ExibirInstrucoesComTransicao());
        }

        if (taNoEventoInstrucoes && Input.GetKeyDown(KeyCode.Escape)) // Tecla Escape para fechar as instruções
        {
            taNoEventoInstrucoes = false;
            StopAllCoroutines(); // Para todas as corrotinas ativas
            ResetarTransicao(); // Volta ao estado inicial
        }
    }

    IEnumerator ExibirInstrucoesComTransicao()
    {
        // Gradualmente escurece a tela
        float alpha = 0;
        while (alpha < 0.5f) // Escurece até 50% (alpha 0.5)
        {
            alpha += Time.deltaTime * velocidadeTransicao;
            telaPreta.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Espera 2 segundos antes de exibir as instruções
        yield return new WaitForSeconds(2f);

        // Exibir a primeira instrução
        MostrarInstrucao(indiceAtual);
        painelInstrucoes.SetActive(true);
    }

    public void ResetarTransicao()
    {
        telaPreta.color = new Color(0, 0, 0, 0); // Volta a tela para transparente
        painelInstrucoes.SetActive(false); // Esconde o painel
    }

    // Mostra a instrução atual baseada no índice
    private void MostrarInstrucao(int index)
    {
        if (index >= 0 && index < listaInstrucoes.Count)
        {
            imagemTecla.sprite = listaInstrucoes[index].imagemTecla;
            descricaoAcao.text = listaInstrucoes[index].descricaoAcao;
        }

        // Verifica se os botões "Anterior" ou "Próximo" devem estar ativos
        botaoAnterior.gameObject.SetActive(index > 0);
        botaoProximo.gameObject.SetActive(index < listaInstrucoes.Count - 1);
    }

    // Método para mostrar a próxima instrução
    public void MostrarProximaInstrucao()
    {
        if (indiceAtual < listaInstrucoes.Count - 1)
        {
            indiceAtual++;
            MostrarInstrucao(indiceAtual);
        }
    }

    // Método para mostrar a instrução anterior
    public void MostrarInstrucaoAnterior()
    {
        if (indiceAtual > 0)
        {
            indiceAtual--;
            MostrarInstrucao(indiceAtual);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = true;
            BotaoInteracao.SetActive(true); // Pode mostrar uma dica de "Pressione F1 para ver instruções"
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            BotaoInteracao.SetActive(false);
        }
    }
}
