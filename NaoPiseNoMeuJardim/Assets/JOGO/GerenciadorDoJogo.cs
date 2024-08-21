using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDoJogo : MonoBehaviour
{
    [Header("Scripts")]
    public TransicaoDeCenas transicaoCenas;
    private ScriptPersonagem personagemScript;

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
    }


    //clicar em Novo Jogo
    public void novoJogo(){
        transicaoCenas.CarregarCena("primeiroAndar");
    }
    
    public void abrirOpcoes(){
        MenuPrincipal.SetActive(false);
        MenuOpcoes.SetActive(true);
    }
    public void fecharOpcoes(){
        MenuOpcoes.SetActive(false);
        MenuPrincipal.SetActive(true);
    }

    //Abrir e fechar menu pause
    public void abrirMenuPause(){
        MenuPause.SetActive(true);
    }
    public void fecharMenuPause(){
        MenuPause.SetActive(false);
    }
    

    //sair do evento do telefone
    public void SairEvento()
    {
        personagemScript.enabled = true;
        SairDoEventoTelefone.SetActive(false);
        personagemScript.RestaurarAnimacoes();
    }
}
