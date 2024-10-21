using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemDicionario
{
    public Sprite imagemItem;   // Imagem do item
    [TextArea] public string descricaoItem; // Descrição do item
}

public class TelefoneEvent : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject ESCbutton;
    public GameObject eButton;
    public GameObject qButton;

    public GameObject BotaoInteracao;
    public GameObject telefoneEventoEntrar;
    public GameObject telefoneEventoSair;

    // Elementos adicionais para a tela preta e o dicionário de itens
    public Image telaPreta;
    public GameObject painelDicionario;
    public Image imagemItem;
    public TextMeshProUGUI descricaoItem;
    public Button botaoProximo;
    public Button botaoAnterior;

    [Header("Configurações")]
    public List<ItemDicionario> itensDicionario; // Lista de itens do dicionário
    public float velocidadeTransicao = 0.5f; // Controle de velocidade da transição da tela preta
    public int indiceAtual = 0; // Índice do item atualmente mostrado

    [Header("Script")]
    private ScriptPersonagem personagemScript;

    [Header("Booleanos")]
    public bool EventoIniciado = false;
    public bool eventoLigado = false;
    public bool taNoEventoTelefone = false;

    public bool painelLigado = false;

    bool dicionarioTaLigado = false;


    private void Start()
    {
        ESCbutton.SetActive(false);
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEventoEntrar.SetActive(false);
        telaPreta.color = new Color(0, 0, 0, 0); // Iniciar com tela transparente
        painelDicionario.SetActive(false); // O painel começa invisível
        indiceAtual = 0;
    }

    private void Update()
    {
        EventoTelefone();

        MostrarProximoItem();
    }

    public void EventoTelefone()
    {
        if (Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
        {
            taNoEventoTelefone = true;
            telefoneEventoEntrar.SetActive(true);
            StartCoroutine(ExibirDicionarioComTransicao());

            if (EventoIniciado)
            {
                personagemScript.DesativarAnimacoes();
                personagemScript.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && eventoLigado == true)
        {
            taNoEventoTelefone = false;
            telefoneEventoEntrar.SetActive(false);
            StopAllCoroutines(); // Para todas as corrotinas ativas
            ResetarTransicao(); // Volta ao estado inicial
            personagemScript.enabled = true;
            personagemScript.RestaurarAnimacoes();
            indiceAtual = -1;
        }
    }

    IEnumerator ExibirDicionarioComTransicao()
    {
        yield return null;

        // Exibir o primeiro item do dicionário
        MostrarItem(indiceAtual);
        painelDicionario.SetActive(true);
        painelLigado = true;
        ESCbutton.SetActive(true);
        qButton.SetActive(true);
        eButton.SetActive(true);
    }

    public void ResetarTransicao()
    {
        telaPreta.color = new Color(0, 0, 0, 0); // Volta a tela para transparente
        painelDicionario.SetActive(false); // Esconde o painel
    }

    // Mostra o item atual baseado no índice
    private void MostrarItem(int index)
    {
        if (index >= 0 && index < itensDicionario.Count)
        {
            imagemItem.sprite = itensDicionario[index].imagemItem;
            descricaoItem.text = itensDicionario[index].descricaoItem;
        }
    }

    // Método para mostrar o próximo item
    public void MostrarProximoItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && taNoEventoTelefone == true)
        {
            MostrarItem(indiceAtual);
            if (indiceAtual < itensDicionario.Count - 1)
            {
                if (painelLigado == true)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                        Debug.Log("ai");
                    indiceAtual++;
                }
            }
        }
        else if (indiceAtual > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) && taNoEventoTelefone == true)
            {
                indiceAtual--;
                MostrarItem(indiceAtual);
            }
        }
    }

    public void SairDoEvento()
    {
        telefoneEventoEntrar.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = true;
            EventoIniciado = true;
            BotaoInteracao.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventoLigado = false;
            EventoIniciado = false;
            BotaoInteracao.SetActive(false);
        }
    }
}
