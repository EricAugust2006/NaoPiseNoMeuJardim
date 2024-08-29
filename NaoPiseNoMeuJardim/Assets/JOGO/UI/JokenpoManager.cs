using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using TMPro;

public class JokenpoManager : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject jokenpo;
    public GameObject resultado;

    [Header("Images")]
    public Image jogadorImagemEscolha;
    public Image computadorImagemEscolha;

    [Header("Sprites")]
    public Sprite pedraSprite;
    public Sprite papelSprite;
    public Sprite tesouraSprite;

    [Header("Texts")]
    public Text textoResultado;

    [Header("Scripts")]
    public ScriptMae mae;
    private SistemaDeVida sistemaDeVida;

    [Header("Scripts")]
    private Animator anim;

    private string[] chances = { "Pedra", "Papel", "Tesoura" };

    private void Start() 
    {
        resultado.SetActive(false);
        anim = GetComponent<Animator>();
        sistemaDeVida = FindObjectOfType<SistemaDeVida>();
    }

    public void jogadorFazerEscolha(string jogadorEscolha)
    {
        // Definir a imagem do jogador
        switch (jogadorEscolha)
        {
            case "Pedra":
                jogadorImagemEscolha.sprite = pedraSprite;
                break;

            case "Papel":
                jogadorImagemEscolha.sprite = papelSprite;
                break;

            case "Tesoura":
                jogadorImagemEscolha.sprite = tesouraSprite;
                break;
        }

        // escolha do compiuter
        string computadorChance = chances[Random.Range(0, chances.Length)];

        // Definir a imagem do compiuter
        switch (computadorChance)
        {
            case "Pedra":
                computadorImagemEscolha.sprite = pedraSprite;
                break;
            case "Papel":
                computadorImagemEscolha.sprite = papelSprite;
                break;
            case "Tesoura":
                computadorImagemEscolha.sprite = tesouraSprite;
                break;
        }

        string result = determinaVencedor(jogadorEscolha, computadorChance);
        textoResultado.text = result;
    }

    public string determinaVencedor(string jogadorEscolha, string computadorChance)
    {
        if (jogadorEscolha == computadorChance)
        {
            StartCoroutine(paralisarMaeAoEmpatar());
            Time.timeScale = 1f;
            Debug.Log("EMPATE");
            return "Empate!";
        }
        else if ((jogadorEscolha == "Pedra" && computadorChance == "Tesoura") ||
                 (jogadorEscolha == "Papel" && computadorChance == "Pedra") ||
                 (jogadorEscolha == "Tesoura" && computadorChance == "Papel"))
        {
            Time.timeScale = 1f;
            jokenpo.SetActive(false);
            Debug.Log("GANHOU");
            return "Você Ganhou!";
        }
        else
        {
            StartCoroutine(paralisarMaeAoPerder());
            Time.timeScale = 1f;    
            jokenpo.SetActive(false);
            sistemaDeVida.vida--;
            Debug.Log("PERDEU");
            return "Você Perdeu!";
        }
    }

    IEnumerator paralisarMaeAoPerder(){
        mae.GetComponent<ScriptMae>().enabled = false;
        jokenpo.SetActive(false);
        mae.speed = 380f;

        StartCoroutine(tempoParaVoltarResultado());
        yield return new WaitForSeconds(4f);
        
        mae.speed = 400f;
        mae.GetComponent<ScriptMae>().enabled = true;
    }

    IEnumerator paralisarMaeAoEmpatar(){
        mae.GetComponent<ScriptMae>().enabled = false;
        jokenpo.SetActive(false);
        mae.speed = 350f;

        StartCoroutine(tempoParaVoltarResultado());
        yield return new WaitForSeconds(5f);

        mae.speed = 400f;
        mae.GetComponent<ScriptMae>().enabled = true;
    }

    IEnumerator paralisarMaeAoGanhar(){
        mae.GetComponent<ScriptMae>().enabled = false;
        jokenpo.SetActive(false);
        mae.speed = 320f;

        StartCoroutine(tempoParaVoltarResultado());
        yield return new WaitForSeconds(7f);

        mae.speed = 400f;
        mae.GetComponent<ScriptMae>().enabled = true;
    }

    IEnumerator tempoParaVoltarResultado(){
        resultado.SetActive(true);
        yield return new WaitForSeconds(3f);
        resultado.SetActive(false);
    }
    // void tempoParaVoltarResultado(){
    //     float tempoVoltar = 0; 
    //     tempoVoltar += Time.deltaTime;

    //     if(tempoVoltar >= 3){
    //         taPodendoAtivar = true;
    //     } else {
    //         taPodendoAtivar = false;
    //     }
    // }

}
