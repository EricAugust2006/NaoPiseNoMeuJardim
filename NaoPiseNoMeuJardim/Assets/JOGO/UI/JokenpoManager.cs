using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class JokenpoManager : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject jokenpo;

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

    private string[] chances = { "Pedra", "Papel", "Tesoura" };

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
            jokenpo.SetActive(false);
            return "Empate!";
        }
        else if ((jogadorEscolha == "Pedra" && computadorChance == "Tesoura") ||
                 (jogadorEscolha == "Papel" && computadorChance == "Pedra") ||
                 (jogadorEscolha == "Tesoura" && computadorChance == "Papel"))
        {
            StartCoroutine(paralisarMaeAoGanhar());
            Time.timeScale = 1f;
            jokenpo.SetActive(false);
            return "Você Ganhou!";
        }
        else
        {
            StartCoroutine(paralisarMaeAoPerder());
            Time.timeScale = 1f;    
            jokenpo.SetActive(false);
            return "Você Perdeu!";
        }
    }

    IEnumerator paralisarMaeAoPerder(){
        mae.GetComponent<ScriptMae>().enabled = false;
        mae.obstacleDetector.enabled = false;
        mae.speed = 380f;

        yield return new WaitForSeconds(4f);
        
        mae.speed = 400f;
        mae.obstacleDetector.enabled = false;
        mae.GetComponent<ScriptMae>().enabled = true;
    }

    IEnumerator paralisarMaeAoEmpatar(){
        mae.GetComponent<ScriptMae>().enabled = false;
        mae.obstacleDetector.enabled = false;
        mae.speed = 350f;

        yield return new WaitForSeconds(5f);

        mae.speed = 400f;
        mae.obstacleDetector.enabled = true;
        mae.GetComponent<ScriptMae>().enabled = true;
    }

    IEnumerator paralisarMaeAoGanhar(){
        mae.GetComponent<ScriptMae>().enabled = false;
        mae.obstacleDetector.enabled = false;
        mae.speed = 320f;

        yield return new WaitForSeconds(7f);

        mae.speed = 400f;
        mae.obstacleDetector.enabled = true;       
        mae.GetComponent<ScriptMae>().enabled = true;
    }

}
