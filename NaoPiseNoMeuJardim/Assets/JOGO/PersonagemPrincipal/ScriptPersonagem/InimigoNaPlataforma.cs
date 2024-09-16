using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class InimigoNaPlataforma : MonoBehaviour
{
    public GameObject inimigoPrefab; // Prefab do inimigo
    public Tilemap plataformaTilemap; // O Tilemap das plataformas
    public LayerMask layerPlataforma; // Layer das plataformas
    public float tempoParaGerarInimigo = 5f; // Tempo necess�rio para gerar o inimigo
    public float tempoParaDesaparecerInimigo = 3f; // Tempo at� o inimigo desaparecer ap�s spawnar
    public TextMeshProUGUI contadorUI; // Texto para mostrar o contador de tempo usando TextMeshPro
    public Animator inimigoAnimator; // Anima��o do inimigo

    private float tempoEmPlataforma = 0f;
    private bool emPlataforma = false;
    private Vector3 plataformaAtualPosicao;
    private bool inimigoAtivo = false; // Controla se h� um inimigo ativo

    private void Update()
    {
        if (emPlataforma && !inimigoAtivo) // S� come�a a contagem se n�o houver inimigo ativo
        {
            // Incrementa o tempo que o player est� sobre a plataforma
            tempoEmPlataforma += Time.deltaTime;

            // Atualiza o contador visual
            AtualizarContador(tempoParaGerarInimigo - tempoEmPlataforma);

            // Se o tempo for maior ou igual ao necess�rio, gera o inimigo
            if (tempoEmPlataforma >= tempoParaGerarInimigo)
            {
                GerarInimigo();
                tempoEmPlataforma = 0f; // Reinicia o tempo
            }
        }
        else if (!emPlataforma)
        {
            tempoEmPlataforma = 0f; // Reseta o tempo quando sai da plataforma
            AtualizarContador(tempoParaGerarInimigo); // Reseta o contador visual
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            emPlataforma = true;
            plataformaAtualPosicao = CalcularPosicaoPlataforma(transform.position); // Pega a posi��o da plataforma
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            emPlataforma = false;
        }
    }

    // Ajusta a posi��o para garantir que o inimigo nas�a no topo da plataforma
    private Vector3 CalcularPosicaoPlataforma(Vector3 posicaoPlayer)
    {
        // Converte a posi��o do player para a c�lula da plataforma no Tilemap
        Vector3Int celulaPlataforma = plataformaTilemap.WorldToCell(posicaoPlayer);
        Vector3 posicaoCentroPlataforma = plataformaTilemap.GetCellCenterWorld(celulaPlataforma);

        // Agora vamos garantir que o inimigo nas�a no ch�o da plataforma com Raycast
        RaycastHit2D hit = Physics2D.Raycast(posicaoCentroPlataforma, Vector2.down, Mathf.Infinity, layerPlataforma);

        if (hit.collider != null)
        {
            // Se o Raycast acertar o ch�o da plataforma, ajustamos a posi��o Y
            posicaoCentroPlataforma.y = hit.point.y;
        }

        return posicaoCentroPlataforma;
    }

    private void GerarInimigo()
    {
        // Marca que h� um inimigo ativo
        inimigoAtivo = true;

        // Gera o inimigo na posi��o da plataforma onde o player est�
        GameObject inimigo = Instantiate(inimigoPrefab, plataformaAtualPosicao, Quaternion.identity);

        // Inicia a anima��o de spawn
        inimigoAnimator = inimigo.GetComponent<Animator>();
        if (inimigoAnimator != null)
        {
            inimigoAnimator.SetTrigger("Spawn"); // Assume que existe um trigger de anima��o "Spawn"
        }

        Debug.Log("Inimigo Gerado!");

        // Despawner inimigo ap�s um tempo
        StartCoroutine(DespawnInimigo(inimigo));
    }

    private IEnumerator DespawnInimigo(GameObject inimigo)
    {
        // Espera o tempo antes de despawnar
        yield return new WaitForSeconds(tempoParaDesaparecerInimigo);

        // Anima��o de despawn
        if (inimigoAnimator != null)
        {
            inimigoAnimator.SetTrigger("Despawn"); // Assume que existe um trigger de anima��o "Despawn"
        }

        // Espera a anima��o terminar
        yield return new WaitForSeconds(1f); // Supondo que a anima��o de despawn dure 1 segundo

        // Destroi o inimigo
        Destroy(inimigo);
        Debug.Log("Inimigo Despawnado!");

        // Reseta o estado do inimigo para permitir gerar outro
        inimigoAtivo = false;
    }

    private void AtualizarContador(float tempoRestante)
    {
        if (contadorUI != null)
        {
            contadorUI.text = Mathf.Ceil(tempoRestante).ToString() + "s"; // Exibe o tempo restante
        }
    }
}
