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
    private int indiceAtual = 0; // Índice do item atualmente mostrado

    [Header("Script")]
    private ScriptPersonagem personagemScript;

    [Header("Booleanos")]
    public bool EventoIniciado = false;
    public bool eventoLigado = false;    
    public bool taNoEventoTelefone = false;

    private void Start()
    {
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEventoEntrar.SetActive(false);
        telaPreta.color = new Color(0, 0, 0, 0); // Iniciar com tela transparente
        painelDicionario.SetActive(false); // O painel começa invisível
        botaoProximo.onClick.AddListener(MostrarProximoItem); // Associa o botão "Próximo" ao método
        botaoAnterior.onClick.AddListener(MostrarItemAnterior); // Associa o botão "Anterior" ao método
    }

    private void Update()
    {
        EventoTelefone();
    }

    public void EventoTelefone()
    {
        if (EventoIniciado && Input.GetKeyDown(KeyCode.E) && eventoLigado == true)
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

        if(EventoIniciado && Input.GetKeyDown(KeyCode.Escape)){
            taNoEventoTelefone = false;
            telefoneEventoEntrar.SetActive(false);
            StopAllCoroutines(); // Para todas as corrotinas ativas
            ResetarTransicao(); // Volta ao estado inicial
            personagemScript.enabled = true;
            personagemScript.RestaurarAnimacoes();
        }
    }

    IEnumerator ExibirDicionarioComTransicao()
    {
        // Gradualmente escurece a tela
        float alpha = 0;
        while (alpha < 0.5f) // Escurece até 50% (alpha 0.5)
        {
            alpha += Time.deltaTime * velocidadeTransicao;
            telaPreta.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Espera 2 segundos antes de exibir o primeiro item
        yield return new WaitForSeconds(2f);

        // Exibir o primeiro item do dicionário
        MostrarItem(indiceAtual);
        painelDicionario.SetActive(true);
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

        // Verifica se os botões "Anterior" ou "Próximo" devem estar ativos
        botaoAnterior.gameObject.SetActive(index > 0);
        botaoProximo.gameObject.SetActive(index < itensDicionario.Count - 1);
    }

    // Método para mostrar o próximo item
    public void MostrarProximoItem()
    {
        if (indiceAtual < itensDicionario.Count - 1)
        {
            indiceAtual++;
            MostrarItem(indiceAtual);
        }
    }

    // Método para mostrar o item anterior
    public void MostrarItemAnterior()
    {
        if (indiceAtual > 0)
        {
            indiceAtual--;
            MostrarItem(indiceAtual);
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
