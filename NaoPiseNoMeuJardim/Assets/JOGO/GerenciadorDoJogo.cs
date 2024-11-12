using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDoJogo : MonoBehaviour
{
    public bool jogoLigado = false;

    [Header("Scripts")]
    public transicaoTeste transicaoteste;
    public TransicaoDeCenas transicaoCenas;
    private ScriptPersonagem personagemScript;
    public TelefoneEvent telefoneEvent;
    public ScriptMae mae;
    private InstrucoesJogo instrucoesJogo;
    private GuiaDoJardim guiaJardim;

    [Header("GameObjects no Menu principal")]
    [SerializeField] GameObject MenuPrincipal;
    [SerializeField] GameObject NovoJogo;
    [SerializeField] GameObject Sair;

    [Header("GameObjects em Opcoes")]
    [SerializeField] GameObject MenuOpcoes;

    [Header("GameObjects em Jogo")]
    [SerializeField] GameObject SairDoEventoTelefone;
    [SerializeField] GameObject MenuPause;


    private void Start()
    {
        guiaJardim = FindObjectOfType<GuiaDoJardim>();
        transicaoCenas = FindObjectOfType<TransicaoDeCenas>();
        instrucoesJogo = FindObjectOfType<InstrucoesJogo>();
        mae = FindObjectOfType<ScriptMae>();
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEvent = FindObjectOfType<TelefoneEvent>();

        if (SceneManager.GetActiveScene().name == "meuQuarto")
        {
            Time.timeScale = 1f;
        }
        if(SceneManager.GetActiveScene().name == "JardimJogo"){
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        
        abrirEfecharMenuPause();
        jogoTaDesligado();
    }

    // ================== MENU PRINCIPAL ===============

    public void novoJogo()
    {
        Time.timeScale = 1f;
        transicaoCenas.CarregarCena("meuQuarto");
    }

    public void irParaQuarto()
    {
        Time.timeScale = 1f;
        transicaoCenas.CarregarCena("meuQuarto");
    }

    public void abrirOpcoes()
    {
        MenuPrincipal.SetActive(false);
        MenuOpcoes.SetActive(true);
    }

    public void fecharOpcoes()
    {
        MenuOpcoes.SetActive(false);
        MenuPrincipal.SetActive(true);
    }

    // ================== EM JOGO ======================

    public void abrirEfecharMenuPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tecla Esc apertada");

            string nomeCenaAtual = SceneManager.GetActiveScene().name;

            if (nomeCenaAtual == "JardimJogo")
            {
                if (!guiaJardim.noMomentoInstrucoes)
                {
                    Debug.Log("Evento de instruções no jardim não está ativo");
                    alternarMenuPause();
                }
            }
            else if (nomeCenaAtual == "primeiroAndar")
            {
                Debug.Log("Cena primeiroAndar ativa");
                if (!telefoneEvent.taNoEventoTelefone)
                {
                    Debug.Log("Evento de telefone não está ativo");
                    alternarMenuPause();
                }
            }
            else if (nomeCenaAtual == "meuQuarto")
            {
                alternarMenuPause();
            }
            else
            {
                alternarMenuPause();
            }
        }
    }

    public void alternarMenuPause()
    {
        bool taAtivo = MenuPause.activeSelf;
        MenuPause.SetActive(!taAtivo);

        if (taAtivo)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void sairParaMenuPrincipal()
    {
        Time.timeScale = 1f;
        transicaoCenas.CarregarCena("MenuPrincipal");
        // SceneManager.LoadScene("MenuPrincipal");
    }

    public void voltarQuarto()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene("meuQuarto");
        transicaoCenas.CarregarCena("meuQuarto");
    }

    // Sair do evento do telefone
    public void SairEvento()
    {
        personagemScript.enabled = true;
        SairDoEventoTelefone.SetActive(false);
        personagemScript.RestaurarAnimacoes();
    }

    public void jogoTaDesligado()
    {
        if (SceneManager.GetActiveScene().name == "MenuPrincipal")
        {
            jogoLigado = false;

            if (!jogoLigado) 
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void recomecarJogo()
    {   
        Time.timeScale = 1f;
        SceneManager.LoadScene("JardimJogo");
        // transicaoCenas.CarregarCena("JardimJogo");
    }

    public void sairDoJogo()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}
