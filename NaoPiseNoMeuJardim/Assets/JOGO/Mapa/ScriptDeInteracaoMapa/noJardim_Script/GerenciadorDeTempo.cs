using UnityEngine;
using UnityEngine.UI; // Necessário para usar UI
using TMPro;

public class GerenciadorDeTempo : MonoBehaviour
{
    public static GerenciadorDeTempo instancia; // Instância única (singleton)
    public float tempoParaAumentar = 30f; // Tempo em segundos para aumentar a taxa de spawn
    private float tempoDecorrido = 0f;
    public float taxaDeSpawnAumentar = 0.1f; // Aumento da taxa de spawn
    public TextMeshProUGUI temporizadorTexto; // Referência ao componente de texto do temporizador
    public GameObject mensagemImagem; // Referência ao objeto da imagem da mensagem
    private float tempoTotal = 300f; // 5 minutos em segundos
    private ScriptPersonagem player;

    void Awake()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        if (instancia == null)
        {
            instancia = this;
            // DontDestroyOnLoad(gameObject); // Mantém este objeto entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player.triggouComTagPararCorrida == true)
        {

            tempoDecorrido += Time.deltaTime;

            // Atualiza o texto do temporizador
            AtualizarTemporizador();

            if (tempoDecorrido >= tempoParaAumentar)
            {
                // Reinicia o tempo decorrido
                tempoDecorrido = 0f;
                // Notifica os outros scripts
                NotificarAumentoTaxaSpawn();
            }

            // Exibe a mensagem quando o tempo total for alcançado
            if (tempoDecorrido >= tempoTotal)
            {
                ExibirMensagemFinalizar();
            }

            // Lógica para finalizar a corrida
            if (mensagemImagem.activeSelf && Input.GetKeyDown(KeyCode.Space)) // Substitua "Space" pela tecla desejada
            {
                // Lógica para finalizar a corrida
                Debug.Log("Corrida finalizada!");
                // Você pode adicionar a lógica para ir para outra cena ou parar o jogo aqui
            }
        }
    }

    void AtualizarTemporizador()
    {
        float tempoRestante = tempoTotal - tempoDecorrido;
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        temporizadorTexto.text = $"{minutos:D2}:{segundos:D2}"; // Formata como mm:ss
    }

    void NotificarAumentoTaxaSpawn()
    {
        // PlataformaGenerator.instancia.AumentarTaxaSpawn(taxaDeSpawnAumentar);
        // ChineloDeMae.instancia.AumentarTaxaSpawn(taxaDeSpawnAumentar);
        // ObstaculoHorizontal.instancia.AumentarTaxaSpawn(taxaDeSpawnAumentar);
        // Adicione outros scripts conforme necessário
    }

    void ExibirMensagemFinalizar()
    {
        mensagemImagem.SetActive(true); // Ativa a imagem com a mensagem
    }
}
