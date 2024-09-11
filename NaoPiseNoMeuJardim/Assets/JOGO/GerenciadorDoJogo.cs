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
        mae = FindObjectOfType<ScriptMae>();
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEvent = FindObjectOfType<TelefoneEvent>();

        if (SceneManager.GetActiveScene().name == "meuQuarto")
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        abrirEfecharMenuPause();
        jogoTaDesligado();
    }

    // =================================================
    // ================== MENU PRINCIPAL ===============
    // =================================================

    public void novoJogo()
    {
        Time.timeScale = 1f;
        transicaoCenas.CarregarCena("meuQuarto");
    }

    public void continuarJogo()
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

    // =================================================
    // ================== EM JOGO ======================
    // =================================================

    public void abrirEfecharMenuPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "JardimJogo")
            {
                if (!mae.jokenpoEventoAtivado)
                {
                    alternarMenuPause();
                }
            }
            else if (SceneManager.GetActiveScene().name == "primeiroAndar")
            {
                if (!telefoneEvent.taNoEventoTelefone)
                {
                    alternarMenuPause();
                }
            }
        }
    }

    public void alternarMenuPause()
    {
        bool taAtivo = MenuPause.activeSelf;
        MenuPause.SetActive(!taAtivo);

        if (taAtivo)
        {
            Time.timeScale = 1f; // Retoma o jogo
        }
        else
        {
            Time.timeScale = 0f; // Pausa o jogo
        }
    }

    public void sairParaMenuPrincipal()
    {
        Time.timeScale = 1f; // Garante que o jogo n�o fique pausado ao sair para o menu
        transicaoCenas.CarregarCena("MenuPrincipal");
    }

    public void voltarQuarto()
    {
        Time.timeScale = 1f; // Garante que o jogo n�o fique pausado ao voltar para o quarto
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

            if (!jogoLigado) // Compara��o correta
            {
                Time.timeScale = 0f; // Pausa o jogo no menu principal
            }
            else
            {
                Time.timeScale = 1f; // Retoma o jogo se n�o estiver no menu principal
            }
        }
    }

    public void recomecarJogo(){
        transicaoCenas.CarregarCena("JardimJogo");
    }

    public void sairDoJogo()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}
