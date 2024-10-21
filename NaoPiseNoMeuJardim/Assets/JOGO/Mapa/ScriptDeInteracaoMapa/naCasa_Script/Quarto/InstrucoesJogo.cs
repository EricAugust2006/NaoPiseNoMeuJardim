using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Instrucao
{
    public Sprite imagemTecla;
    [TextArea] public string descricaoAcao;
}

public class InstrucoesJogo : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject ESCbutton;
    public GameObject eButton;
    public GameObject qButton;

    public GameObject BotaoInteracao;

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
    public bool painelLigado = false;

    private void Start()
    {
        ESCbutton.SetActive(false);
        telaPreta.color = new Color(0, 0, 0, 0); // Iniciar com tela transparente
        painelInstrucoes.SetActive(false); // O painel começa invisível
    }

    private void Update()
    {
        if (eventoLigado && Input.GetKeyDown(KeyCode.E))
        {
            taNoEventoInstrucoes = true;

        }

        if (taNoEventoInstrucoes && Input.GetKeyDown(KeyCode.Escape))
        {
            taNoEventoInstrucoes = false;
            ResetarTransicao();
            indiceAtual = -1;
        }

        MostrarProximoItem();
    }

    private void ExibirInstrucoes()
    {
        // Define a tela preta com transparência
        telaPreta.color = new Color(0, 0, 0, 0.5f);

        // Exibir a primeira instrução
        MostrarInstrucao(indiceAtual);
        painelInstrucoes.SetActive(true); // Exibe o painel de instruções imediatamente
        painelLigado = true;
        ESCbutton.SetActive(true);
        qButton.SetActive(true);
        eButton.SetActive(true);
    }

    public void ResetarTransicao()
    {
        telaPreta.color = new Color(0, 0, 0, 0);
        painelInstrucoes.SetActive(false);
    }

    private void MostrarInstrucao(int index)
    {
        if (index >= 0 && index < listaInstrucoes.Count)
        {
            imagemTecla.sprite = listaInstrucoes[index].imagemTecla;
            descricaoAcao.text = listaInstrucoes[index].descricaoAcao;
        }

        // // Verifica se os botões "Anterior" ou "Próximo" devem estar ativos
        // botaoAnterior.gameObject.SetActive(index > 0);
        // botaoProximo.gameObject.SetActive(index < listaInstrucoes.Count - 1);
    }

    // Método para mostrar a próxima instrução
    public void MostrarProximoItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && taNoEventoInstrucoes == true)
        {
            ExibirInstrucoes();
            MostrarInstrucao(indiceAtual);
            if (indiceAtual < listaInstrucoes.Count - 1)
            {
                if (eventoLigado == true && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("ai");
                    indiceAtual++;
                }
            }
        }
        else if (indiceAtual > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) && taNoEventoInstrucoes == true)
            {
                indiceAtual--;
                MostrarInstrucao(indiceAtual);
            }
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
