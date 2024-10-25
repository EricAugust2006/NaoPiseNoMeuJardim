using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PassoNoJardim
{
    public Sprite iconeFerramenta;
    [TextArea] public string acaoDescricao;
}

public class GuiaDoJardim : MonoBehaviour
{
    [Header("Elementos do Jardim")]
    public GameObject botaoSair;
    public GameObject botaoUsarFerramenta;
    public GameObject botaoRecolher;

    public GameObject dicaInteracao;

    public Image filtroEscurecido;
    public GameObject quadroInstrucoes;
    public Image iconeFerramenta;
    public TextMeshProUGUI textoDescricao;
    public Button botaoAvancar;
    public Button botaoRetroceder;

    [Header("Configurações do Guia")]
    public List<PassoNoJardim> listaPassos; // Lista das instruções de jardinagem
    public float velocidadeDesvanecimento = 0.5f; // Velocidade de desvanecimento do filtro
    private int indiceAtual = 0; // Índice do passo atual no guia

    [Header("Estados do Guia")]
    public bool guiaAtivo = false;
    public bool noMomentoInstrucoes = false;
    public bool quadroVisivel = false;

    private void Start()
    {
        botaoSair.SetActive(false);
        filtroEscurecido.color = new Color(0, 0, 0, 0);
        quadroInstrucoes.SetActive(false);
    }

    private void Update()
    {
        if (guiaAtivo)
        {
            AbrirGuia();
        }
        else 
        {
            ReiniciarFiltro();
        }

        if (noMomentoInstrucoes && Input.GetKeyDown(KeyCode.Escape))
        {
            noMomentoInstrucoes = false;
            ReiniciarFiltro();
            indiceAtual = -1;
        }

        ProximoPasso();
    }

    private void AbrirGuia()
    {
        filtroEscurecido.color = new Color(0, 0, 0, 0.5f);

        MostrarPasso(indiceAtual);
        quadroInstrucoes.SetActive(true);
        quadroVisivel = true;
        botaoSair.SetActive(true);
        botaoRecolher.SetActive(true);
        botaoUsarFerramenta.SetActive(true);
    }

    public void ReiniciarFiltro()
    {
        filtroEscurecido.color = new Color(0, 0, 0, 0);
        quadroInstrucoes.SetActive(false);
    }

    private void MostrarPasso(int index)
    {
        if (index >= 0 && index < listaPassos.Count)
        {
            iconeFerramenta.sprite = listaPassos[index].iconeFerramenta;
            textoDescricao.text = listaPassos[index].acaoDescricao;
        }

        // // Verifica se os botões de navegação devem estar ativos
        // botaoRetroceder.gameObject.SetActive(index > 0);
        // botaoAvancar.gameObject.SetActive(index < listaPassos.Count - 1);
    }

    // Método para mostrar o próximo passo
    public void ProximoPasso()
    {
        if (Input.GetKeyDown(KeyCode.E) && noMomentoInstrucoes)
        {
            AbrirGuia();
            MostrarPasso(indiceAtual);
            if (indiceAtual < listaPassos.Count - 1)
            {
                if (guiaAtivo && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Avançando no guia do jardim");
                    indiceAtual++;
                }
            }
        }
        else if (indiceAtual > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) && noMomentoInstrucoes)
            {
                indiceAtual--;
                MostrarPasso(indiceAtual);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guiaAtivo = true;
            dicaInteracao.SetActive(true); // Pode mostrar uma dica de "Pressione F1 para ver instruções"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guiaAtivo = false;
            dicaInteracao.SetActive(false);
        }
    }
}
