using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    // Músicas para cada cena
    public AudioClip menuPrincipalMusic;
    public AudioClip meuQuartoMusic;
    public AudioClip segundoAndarMusic;
    public AudioClip primeiroAndarMusic;
    public AudioClip jardimJogoMusic;
    public AudioClip pararCorridaMusic;  // Música para quando o trigger for ativado

    private bool isPararCorridaTriggered = false; // Controle interno para o trigger

    void Awake()
    {
        // Singleton para garantir que apenas uma instância exista
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Inscreve o método quando a cena for carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Desinscreve o método para evitar erros
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método chamado toda vez que uma nova cena é carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    // Método para tocar a música correta com base na cena atual
    void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "MenuPrincipal")
        {
            PlayMusic(menuPrincipalMusic);
        }
        else if (sceneName == "MeuQuarto" || sceneName == "SegundoAndar" || sceneName == "primeiroAndar")
        {
            PlayMusic(meuQuartoMusic);  // Trilha única para essas cenas
        }
        else if (sceneName == "JardimJogo" && !isPararCorridaTriggered)
        {
            PlayMusic(jardimJogoMusic);  // Toca até o trigger ser ativado
        }
    }

    // Método para tocar a música desejada
    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Método que verifica se o Player trigou com o "pararCorrida"
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            // Busca o Player e verifica o estado da variável
            var player = FindObjectOfType<ScriptPersonagem>(); // Substitua pelo nome correto do script do Player

            if (player != null && player.triggouComTagPararCorrida && !isPararCorridaTriggered)
            {
                TriggerPararCorrida();
            }
        }
    }

    // Método para trocar a música quando o trigger é ativado
    public void TriggerPararCorrida()
    {
        isPararCorridaTriggered = true;
        PlayMusic(pararCorridaMusic);
    }
}
