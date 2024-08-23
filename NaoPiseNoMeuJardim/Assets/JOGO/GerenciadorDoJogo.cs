using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDoJogo : MonoBehaviour
{

    public bool jogoLigado = false;

    [Header("Scripts")]
    public TransicaoDeCenas transicaoCenas;
    private ScriptPersonagem personagemScript;
    private TelefoneEvent telefoneEvent;

    [Header("GameObjects no Menu principal")]
    [SerializeField] GameObject MenuPrincipal;
    [SerializeField] GameObject NovoJogo;

    [Header("GameObjects em Opcoes")]
    [SerializeField] GameObject MenuOpcoes;

    [Header("GameObjects em Jogo")]
    [SerializeField] GameObject SairDoEventoTelefone;
    [SerializeField] GameObject MenuPause;

    private void Start()
    {
        personagemScript = FindObjectOfType<ScriptPersonagem>();
        telefoneEvent = FindObjectOfType<TelefoneEvent>();
    }   

    private void Update() {
        abrirEfecharMenuPause();
        jogoTaDesligado();

    }

    // =================================================
    // ================== MENU PRINCIPAL ===============
    // =================================================

    public void novoJogo(){
        transicaoCenas.CarregarCena("primeiroAndar");
        Time.timeScale = 0f;
        // jogoTaDesligado();

    }
    public void continuarJogo(){
        transicaoCenas.CarregarCena("primeiroAndar");
        Time.timeScale = 0f;
        // jogoTaDesligado();

    }

    public void abrirOpcoes(){
        MenuPrincipal.SetActive(false);
        MenuOpcoes.SetActive(true);
        // jogoTaDesligado();

    }
    public void fecharOpcoes(){
        MenuOpcoes.SetActive(false);
        MenuPrincipal.SetActive(true);
        // jogoTaDesligado();

    }

    // =================================================
    // ================== EM JOGO ======================
    // =================================================

    public void abrirEfecharMenuPause(){
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            alternarMenuPause();
        }
        // jogoTaDesligado();

    }

    public void alternarMenuPause(){
        bool taAtivo = MenuPause.activeSelf;
        MenuPause.SetActive(!taAtivo);

        if(taAtivo){
            Time.timeScale = 1f;
        }
        else {
            Time.timeScale = 0f;
        }
        // jogoTaDesligado();

    }

    public void sairParaMenuPrincipal(){
        transicaoCenas.CarregarCena("MenuPrincipal");       
        // jogoTaDesligado();

    }

    //sair do evento do telefone
    public void SairEvento()
    {
        // jogoTaDesligado();
        personagemScript.enabled = true;
        SairDoEventoTelefone.SetActive(false);
        personagemScript.RestaurarAnimacoes();
    }

    public void jogoTaDesligado(){
        if(SceneManager.GetActiveScene().name == "MenuPrincipal"){
            jogoLigado = false;

            if(jogoLigado = false){
                Time.timeScale = 0f;
            }
            else {
                Time.timeScale = 1f;
            }
        }
    }

}
