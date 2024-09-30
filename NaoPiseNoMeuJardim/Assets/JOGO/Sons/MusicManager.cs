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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            Debug.Log("AudioSource encontrado e MusicManager inicializado.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cena carregada: " + scene.name);
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "MenuPrincipal")
        {
            PlayMusic(menuPrincipalMusic);
        }
        else if (sceneName == "meuQuarto" || sceneName == "SegundoAndar" || sceneName == "primeiraAndar")
        {
            PlayMusic(meuQuartoMusic);
        }
        else if (sceneName == "JardimJogo" && !isPararCorridaTriggered)
        {
            PlayMusic(jardimJogoMusic);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log("Música tocando: " + clip.name);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            var player = FindObjectOfType<ScriptPersonagem>();

            if (player != null && player.triggouComTagPararCorrida && !isPararCorridaTriggered)
            {
                TriggerPararCorrida();
            }
        }
    }

    public void TriggerPararCorrida()
    {
        isPararCorridaTriggered = true;
        PlayMusic(pararCorridaMusic);
        Debug.Log("Trigger ativado! Mudando para a música: " + pararCorridaMusic.name);
    }
}
