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
    public float tempoParaGerarInimigo = 5f; // Tempo necessário para gerar o inimigo
    public float tempoParaDesaparecerInimigo = 3f; // Tempo até o inimigo desaparecer após spawnar
    public TextMeshProUGUI contadorUI; // Texto para mostrar o contador de tempo usando TextMeshPro
    public Animator inimigoAnimator; // Animação do inimigo

    private float tempoEmPlataforma = 0f;
    private bool emPlataforma = false;
    private Vector3 plataformaAtualPosicao;
    private bool inimigoAtivo = false; // Controla se há um inimigo ativo

    private void Update()
    {
        if (emPlataforma && !inimigoAtivo) // Só começa a contagem se não houver inimigo ativo
        {
            // Incrementa o tempo que o player está sobre a plataforma
            tempoEmPlataforma += Time.deltaTime;

            // Atualiza o contador visual
            AtualizarContador(tempoParaGerarInimigo - tempoEmPlataforma);

            // Se o tempo for maior ou igual ao necessário, gera o inimigo
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
            plataformaAtualPosicao = CalcularPosicaoPlataforma(transform.position); // Pega a posição da plataforma
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            emPlataforma = false;
        }
    }

    // Ajusta a posição para garantir que o inimigo nasça no topo da plataforma
    private Vector3 CalcularPosicaoPlataforma(Vector3 posicaoPlayer)
    {
        // Converte a posição do player para a célula da plataforma no Tilemap
        Vector3Int celulaPlataforma = plataformaTilemap.WorldToCell(posicaoPlayer);
        Vector3 posicaoCentroPlataforma = plataformaTilemap.GetCellCenterWorld(celulaPlataforma);

        // Agora vamos garantir que o inimigo nasça no chão da plataforma com Raycast
        RaycastHit2D hit = Physics2D.Raycast(posicaoCentroPlataforma, Vector2.down, Mathf.Infinity, layerPlataforma);

        if (hit.collider != null)
        {
            // Se o Raycast acertar o chão da plataforma, ajustamos a posição Y
            posicaoCentroPlataforma.y = hit.point.y;
        }

        return posicaoCentroPlataforma;
    }

    private void GerarInimigo()
    {
        // Marca que há um inimigo ativo
        inimigoAtivo = true;

        // Gera o inimigo na posição da plataforma onde o player está
        GameObject inimigo = Instantiate(inimigoPrefab, plataformaAtualPosicao, Quaternion.identity);

        // Inicia a animação de spawn
        inimigoAnimator = inimigo.GetComponent<Animator>();
        if (inimigoAnimator != null)
        {
            inimigoAnimator.SetTrigger("Spawn"); // Assume que existe um trigger de animação "Spawn"
        }

        Debug.Log("Inimigo Gerado!");

        // Despawner inimigo após um tempo
        StartCoroutine(DespawnInimigo(inimigo));
    }

    private IEnumerator DespawnInimigo(GameObject inimigo)
    {
        // Espera o tempo antes de despawnar
        yield return new WaitForSeconds(tempoParaDesaparecerInimigo);

        // Animação de despawn
        if (inimigoAnimator != null)
        {
            inimigoAnimator.SetTrigger("Despawn"); // Assume que existe um trigger de animação "Despawn"
        }

        // Espera a animação terminar
        yield return new WaitForSeconds(1f); // Supondo que a animação de despawn dure 1 segundo

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
