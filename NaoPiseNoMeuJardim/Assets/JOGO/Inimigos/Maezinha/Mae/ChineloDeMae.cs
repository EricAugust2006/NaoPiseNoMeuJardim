using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChineloDeMae : MonoBehaviour
{
    public GameObject chineloPrefab;
    public Transform spawnPointGround;
    public Transform spawnPointPlatform;
    public Transform destinoChao;
    public Transform destinoPlataforma;
    public float tempoAviso = 2f;
    public float velocidadeChinelo = 5f;
    public float tempoMinSpawn = 6f;
    public float tempoMaxSpawn = 12f;
    public GameObject avisoUI;

    [Range(0f, 1f)]
    public float chanceSpawn = 0.5f; // Chance inicial de spawn
    public float aumentoChance = 0.1f; // O valor de aumento da chance
    public float maxChance = 1f; // Valor máximo da chance de spawn

    private ScriptPersonagem player;

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        avisoUI.SetActive(false);
        StartCoroutine(SpawnChinelo());
    }

    private IEnumerator SpawnChinelo()
    {
        while (true)
        {
            // Verifica se o player triggou a condição
            if (player.triggouComTagPararCorrida)
            {
                if (Random.value <= chanceSpawn)
                {
                    float intervaloSpawn = Random.Range(tempoMinSpawn, tempoMaxSpawn);
                    yield return new WaitForSeconds(intervaloSpawn);

                    avisoUI.SetActive(true);
                    Debug.Log("Cuidado! Um chinelo vai chegar!");
                    yield return new WaitForSeconds(tempoAviso);

                    avisoUI.SetActive(false);
                    StartCoroutine(MoverChinelo());
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                }
            }
            else
            {
                yield return null; // Espera até que a condição seja atendida novamente
            }
        }
    }

    private IEnumerator MoverChinelo()
    {
        GameObject chineloLancado = Instantiate(
            chineloPrefab,
            spawnPointGround.position,
            Quaternion.identity
        );

        while (chineloLancado != null
            && Vector3.Distance(chineloLancado.transform.position, destinoChao.position) > 0.1f)
        {
            chineloLancado.transform.position = Vector3.MoveTowards(
                chineloLancado.transform.position,
                destinoChao.position,
                velocidadeChinelo * Time.deltaTime
            );
            yield return null;
        }

        if (chineloLancado != null)
        {
            Destroy(chineloLancado);
        }

        yield return new WaitForSeconds(0.5f);

        GameObject chineloNaPlataforma = Instantiate(
            chineloPrefab,
            spawnPointPlatform.position,
            Quaternion.identity
        );
        chineloNaPlataforma.transform.position = new Vector3(
            chineloNaPlataforma.transform.position.x,
            spawnPointPlatform.position.y,
            chineloNaPlataforma.transform.position.z
        );

        while (chineloNaPlataforma != null
            && Vector3.Distance(chineloNaPlataforma.transform.position, destinoPlataforma.position)
                > 0.1f)
        {
            chineloNaPlataforma.transform.position = Vector3.MoveTowards(
                chineloNaPlataforma.transform.position,
                destinoPlataforma.position,
                velocidadeChinelo * Time.deltaTime
            );
            yield return null;
        }

        if (chineloNaPlataforma != null)
        {
            Destroy(chineloNaPlataforma);
        }
    }

    // Função pública para aumentar a chance de spawn
    public void AumentarChance()
    {
        chanceSpawn += aumentoChance;
        chanceSpawn = Mathf.Clamp(chanceSpawn, 0f, maxChance);
        Debug.Log($"Chance de spawn aumentada para: {chanceSpawn}");
    }
}
