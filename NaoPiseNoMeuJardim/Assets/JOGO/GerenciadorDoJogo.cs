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
    }   

    public void Update() {
        abrirEfecharMenuPause();
        jogoTaDesligado();
    }

    // =================================================
    // ================== MENU PRINCIPAL ===============
    // =================================================

    public void novoJogo(){
        transicaoCenas.CarregarCena("meuQuarto");
        Time.timeScale = 0f;
        // jogoTaDesligado();
    }
    public void continuarJogo(){
        transicaoCenas.CarregarCena("meuQuarto");
        Time.timeScale = 0f;
        // jogoTaDesligado();
    }
    public void irParaQuarto(){
        transicaoCenas.CarregarCena("meuQuarto");
        Time.timeScale = 1f;
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
            if(SceneManager.GetActiveScene().name == "JardimJogo"){
                if(mae.jokenpoEventoAtivado == true)
                {
                    return;
                }   
                else {
                    alternarMenuPause();
                }
            }

            if(SceneManager.GetActiveScene().name == "primeiroAndar"){
                if(telefoneEvent.taNoEventoTelefone == true)
                {
                    return;
                }
                else {
                    alternarMenuPause();
                }
            }
        }
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
    }

    public void sairParaMenuPrincipal(){
        transicaoCenas.CarregarCena("MenuPrincipal");       
    }

    public void voltarQuarto(){
        transicaoCenas.CarregarCena("MeuQuarto");
    }

    //sair do evento do telefone
    public void SairEvento()
    {
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

    public void sairDoJogo(){
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}
