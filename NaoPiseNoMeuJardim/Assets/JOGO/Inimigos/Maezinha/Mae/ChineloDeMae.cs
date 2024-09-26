using System.Collections;
using UnityEngine;

public class ChineloDeMae : MonoBehaviour
{
    public GameObject chineloPrefab; // Prefab do chinelo
    public Transform spawnPointGround; // Ponto de spawn no ch�o
    public Transform spawnPointPlatform; // Ponto de spawn nas plataformas
    public Transform destinoChao; // GameObject para o destino final do chinelo no ch�o
    public Transform destinoPlataforma; // GameObject para o destino final do chinelo na plataforma
    public float tempoAviso = 2f; // Tempo de aviso antes do chinelo aparecer
    public float velocidadeChinelo = 5f; // Velocidade do chinelo
    public float tempoMinSpawn = 6f; // Tempo m�nimo de spawn (em segundos)
    public float tempoMaxSpawn = 12f; // Tempo m�ximo de spawn (em segundos)
    [Range(0f, 1f)]
    public float chanceSpawn = 0.5f; // Chance de spawn (0.0 a 1.0)

    public static ChineloDeMae instancia; // Instância única
    public float chanceSpawnInimigo = 0.1f; // Exemplo de chance inicial de spawn
    // public GameObject chineloPrefab; // Prefab do chinelo a ser instanciado
    public float intervaloSpawn = 2f; // Intervalo de spawn

    private ScriptPersonagem player;

    void Start()
    {
        if (instancia == null)
        {
            instancia = this;
            StartCoroutine(SpawnChinelo());
        }
        player = FindObjectOfType<ScriptPersonagem>();
    }

     public void AumentarTaxaSpawn(float aumento)
    {
        chanceSpawnInimigo += aumento;
        // Limite opcional para a taxa de spawn
        chanceSpawnInimigo = Mathf.Clamp(chanceSpawnInimigo, 0f, 1f);
    }

    private IEnumerator SpawnChinelo()
    {
        while (true) // Loop infinito para o spawn cont�nuo
        {
            // Verifica se o chinelo deve ser spawned com base na chance
            if (Random.value <= chanceSpawn)
            {
                // Espera um intervalo aleat�rio entre o tempo m�nimo e m�ximo
                float intervaloSpawn = Random.Range(tempoMinSpawn, tempoMaxSpawn);
                yield return new WaitForSeconds(intervaloSpawn);

                // Avisar o jogador
                Debug.Log("Cuidado! Um chinelo vai chegar!");
                yield return new WaitForSeconds(tempoAviso); // Tempo de aviso

                // Inicia o movimento do chinelo
                StartCoroutine(MoverChinelo());
            }
            else
            {
                // Espera um intervalo aleat�rio antes de verificar novamente
                yield return new WaitForSeconds(Random.Range(1f, 3f)); // Tempo de espera caso n�o spawne
            }
        }
    }

    private IEnumerator MoverChinelo()
    {
        if (player.triggouComTagPararCorrida == true)
        {

            // Cria o chinelo no ponto de spawn do ch�o
            GameObject chineloLancado = Instantiate(chineloPrefab, spawnPointGround.position, Quaternion.identity);

            // Passo 1: Mover por baixo (ch�o)
            while (Vector3.Distance(chineloLancado.transform.position, destinoChao.position) > 0.1f)
            {
                chineloLancado.transform.position = Vector3.MoveTowards(chineloLancado.transform.position, destinoChao.position, velocidadeChinelo * Time.deltaTime);
                yield return null; // Espera at� o pr�ximo frame
            }

            // Destr�i o chinelo quando atinge o destino
            Destroy(chineloLancado);

            // Aguardar um momento antes de gerar o chinelo na plataforma
            yield return new WaitForSeconds(0.5f);

            // Passo 2: Criar o chinelo na plataforma e mover por cima
            GameObject chineloNaPlataforma = Instantiate(chineloPrefab, spawnPointPlatform.position, Quaternion.identity);
            chineloNaPlataforma.transform.position = new Vector3(chineloNaPlataforma.transform.position.x, spawnPointPlatform.position.y, chineloNaPlataforma.transform.position.z); // Ajusta a altura do chinelo

            // Passo 3: Mover o chinelo na plataforma at� o destino
            while (Vector3.Distance(chineloNaPlataforma.transform.position, destinoPlataforma.position) > 0.1f)
            {
                chineloNaPlataforma.transform.position = Vector3.MoveTowards(chineloNaPlataforma.transform.position, destinoPlataforma.position, velocidadeChinelo * Time.deltaTime);
                yield return null; // Espera at� o pr�ximo frame
            }

            // Ap�s atingir o ponto final, destr�i o chinelo na plataforma
            Destroy(chineloNaPlataforma);
        }
    }
}
