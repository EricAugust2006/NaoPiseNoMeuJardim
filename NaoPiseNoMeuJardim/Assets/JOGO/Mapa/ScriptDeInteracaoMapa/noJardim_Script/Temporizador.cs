using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Temporizador : MonoBehaviour
{
    public float intervaloAumento = 30f;
    private ChineloDeMae chineloScript;

    public TextMeshProUGUI tempoUIText;
    public float tempoMaximo = 180f;
    public float tempoAtual = 0f;
    private float proximoAumento;

    public ScriptPersonagem player;

    private Coroutine cronometroCoroutine;
    public GameObject clicarParaTerminarCorrida;
    public GameObject temporizadorGameObject;
    public GameObject menuFimDeJogo;

    public float fadeSpeed = 0.2f; 
    public Image blackScreenPanel;

    private bool corridaFinalizada = false;

    public List<GameObject> paredes;

    public List<GameObject> gameObjectsParaDesabilitar;

    void Start()
    {
        if (gameObjectsParaDesabilitar == null){
            gameObjectsParaDesabilitar = new List<GameObject>();
        }

        if (paredes == null)
        {
            paredes = new List<GameObject>();
        }
        player = FindObjectOfType<ScriptPersonagem>();
        chineloScript = FindObjectOfType<ChineloDeMae>();

        proximoAumento = intervaloAumento;

        
        blackScreenPanel.gameObject.SetActive(true);
        Color color = blackScreenPanel.color;
        color.a = 0f; 
        blackScreenPanel.color = color;

        menuFimDeJogo.SetActive(false);
    }

    void Update()
    {
        if (player.triggouComTagPararCorrida == true)
        {
            temporizadorGameObject.SetActive(true);
        }

        if (player.triggouComTagPararCorrida == true && cronometroCoroutine == null)
        {
            cronometroCoroutine = StartCoroutine(Cronometro());
        }

        if (tempoAtual >= tempoMaximo && Input.GetKeyDown(KeyCode.F) && !corridaFinalizada)
        {
            desativarGameObjects();
            FinalizarCorrida();
            desativarColisores();
            player.gameObject.tag = "Vencedor";
        }
    }

    public void desativarGameObjects(){
        foreach(GameObject gameobjectsDesativar in gameObjectsParaDesabilitar)
        {
            if(gameobjectsDesativar != null){
                gameobjectsDesativar.SetActive(false);
            }
        }
    }

    public void ativarColisores()
    {
        foreach (GameObject parede in paredes)
        {
            if (parede != null)
            {
                parede.SetActive(true);
            }
        }
    }

    public void desativarColisores()
    {
        foreach (GameObject parede in paredes)
        {
            if (parede != null)
            {
                parede.SetActive(false);
            }
        }
    }

    private IEnumerator Cronometro()
    {
        while (tempoAtual < tempoMaximo)
        {
            tempoAtual += Time.deltaTime;

            string minutos = Mathf.FloorToInt(tempoAtual / 60).ToString("00");
            string segundos = Mathf.FloorToInt(tempoAtual % 60).ToString("00");

            tempoUIText.text = minutos + ":" + segundos;

            if (tempoAtual >= proximoAumento && chineloScript != null)
            {
                chineloScript.AumentarChance();
                proximoAumento += intervaloAumento;
            }

            yield return null;
        }

        tempoUIText.text = "03:00";
        temporizadorGameObject.SetActive(false);
        clicarParaTerminarCorrida.SetActive(true);
    }

    private void FinalizarCorrida()
    {
        // player.col.enabled = false;
        // player.rb.gravityScale = 1f;
        player.gameObject.layer = LayerMask.NameToLayer("Invencibilidade");
        corridaFinalizada = true;
        clicarParaTerminarCorrida.SetActive(false);
        player.IniciarMovimentoAutomatico();

        StartCoroutine(FinalizarComFade());
    }

    private IEnumerator FinalizarComFade()
    {
        yield return new WaitForSeconds(3f); 

        Color color = blackScreenPanel.color;

        
        while (color.a < 1f)
        {
            color.a += fadeSpeed * Time.deltaTime; 
            blackScreenPanel.color = color;
            yield return null;
        }

        Time.timeScale = 0; 
        menuFimDeJogo.SetActive(true); 
    }
}
