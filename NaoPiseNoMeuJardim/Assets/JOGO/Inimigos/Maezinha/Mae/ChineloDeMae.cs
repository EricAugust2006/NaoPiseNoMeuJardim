using System.Collections;
using UnityEngine;

public class ChineloDeMae : MonoBehaviour
{
    public GameObject chineloPrefab; // Prefab do chinelo
    public Transform spawnPointGround; // Ponto de spawn no chão
    public Transform spawnPointPlatform; // Ponto de spawn nas plataformas
    public Transform destinoChao; // GameObject para o destino final do chinelo no chão
    public Transform destinoPlataforma; // GameObject para o destino final do chinelo na plataforma
    public float tempoAviso = 2f; // Tempo de aviso antes do chinelo aparecer
    public float velocidadeChinelo = 5f; // Velocidade do chinelo
    public float tempoMinSpawn = 6f; // Tempo mínimo de spawn (em segundos)
    public float tempoMaxSpawn = 12f; // Tempo máximo de spawn (em segundos)
    [Range(0f, 1f)]
    public float chanceSpawn = 0.5f; // Chance de spawn (0.0 a 1.0)

    private ScriptPersonagem player;

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        StartCoroutine(SpawnChinelo());
    }

    private IEnumerator SpawnChinelo()
    {
        while (true) // Loop infinito para o spawn contínuo
        {
            // Verifica se o chinelo deve ser spawned com base na chance
            if (Random.value <= chanceSpawn)
            {
                // Espera um intervalo aleatório entre o tempo mínimo e máximo
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
                // Espera um intervalo aleatório antes de verificar novamente
                yield return new WaitForSeconds(Random.Range(1f, 3f)); // Tempo de espera caso não spawne
            }
        }
    }

    private IEnumerator MoverChinelo()
    {
        // Cria o chinelo no ponto de spawn do chão
        GameObject chineloLancado = Instantiate(chineloPrefab, spawnPointGround.position, Quaternion.identity);

        // Passo 1: Mover por baixo (chão)
        while (Vector3.Distance(chineloLancado.transform.position, destinoChao.position) > 0.1f)
        {
            chineloLancado.transform.position = Vector3.MoveTowards(chineloLancado.transform.position, destinoChao.position, velocidadeChinelo * Time.deltaTime);
            yield return null; // Espera até o próximo frame
        }

        // Destrói o chinelo quando atinge o destino
        Destroy(chineloLancado);

        // Aguardar um momento antes de gerar o chinelo na plataforma
        yield return new WaitForSeconds(0.5f);

        // Passo 2: Criar o chinelo na plataforma e mover por cima
        GameObject chineloNaPlataforma = Instantiate(chineloPrefab, spawnPointPlatform.position, Quaternion.identity);
        chineloNaPlataforma.transform.position = new Vector3(chineloNaPlataforma.transform.position.x, spawnPointPlatform.position.y, chineloNaPlataforma.transform.position.z); // Ajusta a altura do chinelo

        // Passo 3: Mover o chinelo na plataforma até o destino
        while (Vector3.Distance(chineloNaPlataforma.transform.position, destinoPlataforma.position) > 0.1f)
        {
            chineloNaPlataforma.transform.position = Vector3.MoveTowards(chineloNaPlataforma.transform.position, destinoPlataforma.position, velocidadeChinelo * Time.deltaTime);
            yield return null; // Espera até o próximo frame
        }

        // Após atingir o ponto final, destrói o chinelo na plataforma
        Destroy(chineloNaPlataforma);
    }
}
