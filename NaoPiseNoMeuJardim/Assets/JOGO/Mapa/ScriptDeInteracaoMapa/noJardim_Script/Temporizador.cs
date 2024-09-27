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

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        // Encontra o script ChineloDeMae na cena
        chineloScript = FindObjectOfType<ChineloDeMae>();

        // Inicializa o próximo aumento
        proximoAumento = intervaloAumento;
    }

    void Update()
    {
        // Inicia o cronômetro apenas quando a variável for true
        if (player.triggouComTagPararCorrida == true && cronometroCoroutine == null)
        {
            cronometroCoroutine = StartCoroutine(Cronometro());
        }
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
    }
}
