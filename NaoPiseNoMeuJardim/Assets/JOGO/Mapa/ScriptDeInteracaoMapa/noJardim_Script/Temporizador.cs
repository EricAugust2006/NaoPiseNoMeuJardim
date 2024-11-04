using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temporizador : MonoBehaviour
{
    public float intervaloAumento = 30f; // Tempo em segundos para acionar o aumento
    private ChineloDeMae chineloScript;

    public TextMeshProUGUI tempoUIText; // Referência ao componente de texto na UI
    public float tempoMaximo = 300f; // 5 minutos (300 segundos)
    public float tempoAtual = 0f; // Tempo atual que será contado
    private float proximoAumento; // Controla quando deve ocorrer o próximo aumento

    // Variável para controlar quando iniciar o cronômetro
    public ScriptPersonagem player;

    private Coroutine cronometroCoroutine;
    public GameObject clicarParaTerminarCorrida;
    public GameObject temporizadorGameObject;

    public float fadeSpeed = .5f;
    public Image blackScreenPanel;


    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        // Encontra o script ChineloDeMae na cena
        chineloScript = FindObjectOfType<ChineloDeMae>();

        // Inicializa o próximo aumento
        proximoAumento = intervaloAumento;


        // Start with black screen active
        blackScreenPanel.gameObject.SetActive(true);
        Color color = blackScreenPanel.color;
        color.a = 1f;
        blackScreenPanel.color = color;
    }

    void Update()
    {
        if (player.triggouComTagPararCorrida == true)
        {
            temporizadorGameObject.SetActive(true);
        }

        // Inicia o cronômetro apenas quando a variável for true
        if (player.triggouComTagPararCorrida == true && cronometroCoroutine == null)
        {
            cronometroCoroutine = StartCoroutine(Cronometro());
        }

        // chamarVovozinha();
    }

    private IEnumerator Cronometro()
    {
        while (tempoAtual < tempoMaximo)
        {
            // Atualiza o tempo atual a cada frame
            tempoAtual += Time.deltaTime;

            // Converte o tempo atual para o formato de minutos e segundos (MM:SS)
            string minutos = Mathf.FloorToInt(tempoAtual / 60).ToString("00");
            string segundos = Mathf.FloorToInt(tempoAtual % 60).ToString("00");

            // Atualiza o texto na UI
            tempoUIText.text = minutos + ":" + segundos;

            // Verifica se já é o momento de aumentar a chance de spawn
            if (tempoAtual >= proximoAumento && chineloScript != null)
            {
                chineloScript.AumentarChance();
                proximoAumento += intervaloAumento; // Agenda o próximo aumento
            }

            yield return null; // Aguarda o próximo frame
        }

        // Após 5 minutos, interrompe o temporizador
        tempoUIText.text = "05:00";
        Debug.Log("Tempo máximo atingido!");

        temporizadorGameObject.SetActive(false);
        clicarParaTerminarCorrida.SetActive(true);
    }

    public void chamarVovozinha()
    {
        if (tempoAtual == tempoMaximo)
        {
            clicarParaTerminarCorrida.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C) && player.triggouComTagPararCorrida == true)
            {
                StartCoroutine(FadeOutBlackScreen());

                IEnumerator FadeOutBlackScreen()
                {
                    Color color = blackScreenPanel.color;

                    // Gradually fade out the black screen
                    while (color.a > 0f)
                    {
                        color.a -= fadeSpeed * Time.deltaTime;
                        blackScreenPanel.color = color;
                        yield return null;
                    }
                    // Disable black screen panel after fade out
                    blackScreenPanel.gameObject.SetActive(false);
                }
                //script para chamar animação para chamar o script do fim da corrida infinita
            }
        }
    }

    public void cagarnaCalca(){
        float cagar = 0;   
    }
}
