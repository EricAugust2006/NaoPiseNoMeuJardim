using System.Collections;
using UnityEngine;
using Pathfinding;

public class ScriptMae : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target; // O alvo que o chinelo vai seguir (o jogador)
    public float activateDistance = 50f; // Distância para ativar o comportamento de perseguição
    public float pathUpdateSeconds = 0.5f; // Tempo de atualização do caminho

    [Header("Physics")]
    public float speed; // Velocidade de movimento da mãe
    public float nextWaypointDistance = 3f; // Distância para o próximo ponto de caminho
    public float jumpForce = 10f; // Força do pulo
    public LayerMask obstacleLayer; // Camada para detectar obstáculos

    [Header("Custom Behavior")]
    public bool followEnabled = false; // Habilitar o comportamento de perseguição
    public bool directionLookEnabled = true; // Habilitar o comportamento de olhar na direção
    private Path path; // Caminho a seguir
    private int currentWayPoint = 0; // Índice do waypoint atual
    private Seeker seeker; // Componente Seeker para calcular o caminho
    private Rigidbody2D rb; // Componente Rigidbody2D da mãe
    public Collider2D obstacleDetector; // Colisor para detectar obstáculos

    [Header("Animator")]
    private Animator animator; // Controlador de animação

    [Header("GameObjects")]
    public GameObject Pedra_Papel_Tesoura; // UI para o minijogo
    public GameObject coliderFicarNoChao; // Colisor para detectar o chão
    public GameObject resultado; // UI de resultado

    [Header("Scripts")]
    private JARDIM jardim; // Referência ao script do Jardim

    [Header("Booleanos")]
    public bool jokenpoEventoAtivado = false; // Se o evento do Jokenpô está ativado
    private bool eventoDesativadoTemporariamente = false; // Para desativar o evento temporariamente

    [Header("Chinelo Settings")]
    public GameObject chineloPrefab; // Prefab do chinelo
    public Transform chineloSpawnPoint; // Ponto de origem do lançamento do chinelo
    public float throwInterval = 5f; // Intervalo de tempo para lançar o chinelo
    public float chineloSpeed = 5f; // Velocidade inicial do chinelo
    public float chineloThrowSpeed = 10f; // Velocidade ao ser arremessado para o jogador
    public float chineloFollowDuration = 3f; // Tempo que o chinelo persegue o jogador

    private bool isChineloFollowing = false; // Controle do estado do chinelo
    private GameObject chineloInstanciado; // Instância do chinelo lançado

    void Start()
    {
        // Inicialização dos componentes e configuração inicial
        GetComponent<Collider2D>().enabled = false; // Desativa o colisor inicial
        coliderFicarNoChao.SetActive(false); // Desativa o colisor de chão
        jardim = FindObjectOfType<JARDIM>(); // Encontra o script JARDIM na cena
        seeker = GetComponent<Seeker>(); // Obtém o componente Seeker para pathfinding
        rb = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D para física
        obstacleDetector = GetComponentInChildren<Collider2D>(); // Obtém o colisor para detectar obstáculos
        animator = GetComponent<Animator>(); // Obtém o controlador de animação

        // Inicia a atualização do caminho e o lançamento do chinelo em intervalos regulares
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        // StartCoroutine(ThrowChineloRoutine());
    }

    private void FixedUpdate()
    {
        // Atualiza a perseguição e animação se o alvo estiver dentro da distância e o comportamento de perseguição estiver ativado
        if (TargetInDistance() && followEnabled && jardim.IniciarJogo == true)
        {
            PathFollow();
            animator.SetBool("taCorrendo", true); // Ativa a animação de correr
        }

        // Atualiza o movimento do chinelo se ele estiver instanciado
        if (chineloInstanciado != null)
        {
            // AtualizarMovimentoChinelo();
        }
    }

    private void UpdatePath()
    {
        // Atualiza o caminho se o comportamento de perseguição estiver ativado e o alvo estiver dentro da distância
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        // Segue o caminho se houver um caminho válido
        if (path == null || currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        // Calcula a direção para o próximo ponto de caminho
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed * Time.deltaTime, rb.linearVelocity.y); // Move o personagem na direção do próximo waypoint

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++; // Avança para o próximo waypoint se o personagem estiver próximo o suficiente
        }

        // Altera a direção do personagem com base na velocidade
        if (directionLookEnabled)
        {
            if (rb.linearVelocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.linearVelocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        // Verifica se o alvo está dentro da distância de ativação
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        // Define o caminho quando o cálculo está completo e sem erros
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Adiciona força de pulo se o objeto colidido estiver na camada de obstáculos
        if (obstacleLayer == (obstacleLayer | (1 << other.gameObject.layer)))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void PerseguirFilho()
    {
        // Habilita o comportamento de perseguição
        followEnabled = true;
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        // Ativa o evento de Jokenpô se o personagem colidir com o jogador e o jogo estiver iniciado
        if (colisao.gameObject.tag == "Player" && jardim.IniciarJogo == true && !eventoDesativadoTemporariamente)
        {
            resultado.SetActive(false);
            jokenpoEventoAtivado = true;
            GetComponent<Collider2D>().enabled = true;
            Pedra_Papel_Tesoura.SetActive(true);
            Time.timeScale = 0f; // Pausa o jogo
        }
    }

    public void DesativarEventoTemporariamente(float duracao)
    {
        // Desativa o evento temporariamente por uma duração específica
        StartCoroutine(DesativarEventoCoroutine(duracao));
    }

    private IEnumerator DesativarEventoCoroutine(float duracao)
    {
        eventoDesativadoTemporariamente = true;
        yield return new WaitForSecondsRealtime(duracao);
        eventoDesativadoTemporariamente = false;
    }

    // // Função que lança o chinelo a cada intervalo
    // private IEnumerator ThrowChineloRoutine()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(throwInterval);
    //         if (jardim.IniciarJogo)
    //         {
    //             ThrowChinelo();
    //         }
    //     }
    // }

    // // Função para lançar o chinelo
    // private void ThrowChinelo()
    // {
    //     chineloInstanciado = Instantiate(chineloPrefab, chineloSpawnPoint.position, chineloSpawnPoint.rotation);
    //     isChineloFollowing = true;
    //     StartCoroutine(ChineloBehaviour());

    //     Destroy(chineloInstanciado, 6f);
    // }

    // // Função para controlar o comportamento do chinelo (perseguir e depois arremessar)
    // public IEnumerator ChineloBehaviour()
    // {
    //     Rigidbody2D chineloRb = chineloInstanciado.GetComponent<Rigidbody2D>();

    //     // Verifica se o chinelo possui um Rigidbody2D
    //     if (chineloRb != null)
    //     {
    //         // Primeira fase: chinelo segue o jogador com velocidade normal
    //         float elapsedTime = 0f;
    //         while (elapsedTime < chineloFollowDuration)
    //         {
    //             if (!isChineloFollowing)
    //                 yield break;

    //             Vector2 direction = (target.position - chineloInstanciado.transform.position).normalized;
    //             chineloRb.velocity = direction * chineloSpeed;
    //             elapsedTime += Time.deltaTime;
    //             yield return null;
    //         }

    //         // Segunda fase: chinelo é arremessado em alta velocidade em direção ao jogador
    //         isChineloFollowing = false;
    //         Vector2 throwDirection = (target.position - chineloInstanciado.transform.position).normalized;
    //         chineloRb.velocity = throwDirection * chineloThrowSpeed;
    //     }
    // }

    // // Função para atualizar o movimento do chinelo
    // private void AtualizarMovimentoChinelo()
    // {
    //     if (chineloInstanciado == null)
    //         return;

    //     Rigidbody2D chineloRb = chineloInstanciado.GetComponent<Rigidbody2D>();
    //     if (chineloRb != null && isChineloFollowing)
    //     {
    //         Vector2 direction = (target.position - chineloInstanciado.transform.position).normalized;
    //         chineloRb.velocity = direction * chineloSpeed;
    //     }
    // }
}
