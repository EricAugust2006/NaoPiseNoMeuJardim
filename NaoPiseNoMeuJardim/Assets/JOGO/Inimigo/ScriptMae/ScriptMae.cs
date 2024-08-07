using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScriptMae : MonoBehaviour
{
    // Vari�veis p�blicas ajust�veis no Inspector
    [Header("Pathfinding")]
    public Transform target; // Alvo que a m�e ir� perseguir
    public float activateDistance = 50f; // Dist�ncia para ativar a persegui��o
    public float pathUpdateSeconds = 0.5f; // Intervalo de atualiza��o do caminho

    [Header("Physics")]
    public float speed = 200f; // Velocidade de movimento
    public float nextWaypointDistance = 3f; // Dist�ncia para o pr�ximo ponto de caminho
    public float jumpNodeHeightRequirement = 1.5f; // Altura necess�ria para pular
    public float jumpModifier = 1.5f; // Modificador de for�a de pulo
    public float jumpCheckOffset = 0.1f; // Offset para verifica��o de pulo
    public LayerMask groundLayer; // Camada do ch�o
    public LayerMask obstacleLayer; // Camada dos obst�culos
    public float circleCastRadius = 1f; // Raio do CircleCast
    public Vector2 circleCastOffset = new Vector2(1f, 0f); // Offset para o CircleCast

    [Header("Custom Behavior")]
    public bool followEnabled = false; // Se a persegui��o est� habilitada
    public bool jumpEnabled = true; // Se o pulo est� habilitado
    public bool directionLookEnabled = true; // Se a dire��o do olhar est� habilitada

    // Vari�veis privadas
    private Path path; // Caminho gerado pelo Seeker
    private int currentWayPoint = 0; // Ponto atual do caminho
    public bool isGrounded = false; // Se est� no ch�o
    Seeker seeker; // Componente Seeker
    Rigidbody2D rb; // Componente Rigidbody2D
    CapsuleCollider2D capsuleCollider; // Componente CapsuleCollider2D
    Collider2D triggerCollider; // Componente Collider2D usado como trigger

    void Start()
    {
        // Inicializa componentes
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        capsuleCollider.enabled = false;
        triggerCollider = GetComponentInChildren<Collider2D>();

        // Repetidamente chama UpdatePath no intervalo definido
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        // Se o alvo est� na dist�ncia e a persegui��o est� habilitada, segue o caminho
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        // Atualiza o caminho se a persegui��o est� habilitada e o alvo est� na dist�ncia
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        // Se n�o h� caminho, retorna
        if (path == null)
        {
            return;
        }

        // Se chegou ao final do caminho, retorna
        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        // Verifica se est� no ch�o
        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, 2f, groundLayer);

        // Calcula a dire��o para o pr�ximo ponto do caminho
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        // Define a velocidade diretamente para manter constante
        rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, rb.velocity.y);

        // Verifica se deve pular
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement || IsObstacleInFront())
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        // Atualiza para o pr�ximo ponto do caminho se a dist�ncia for menor que a permitida
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        // Altera a dire��o do sprite conforme a dire��o do movimento
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    // Verifica se o alvo est� dentro da dist�ncia de ativa��o
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    // Callback quando o caminho � completo
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Verifica se h� um obst�culo na frente usando CircleCast
    private bool IsObstacleInFront()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 circleCastPosition = (Vector2)transform.position + new Vector2(circleCastOffset.x * direction, circleCastOffset.y);
        RaycastHit2D hit = Physics2D.CircleCast(circleCastPosition, circleCastRadius, Vector2.right * direction, nextWaypointDistance, obstacleLayer);
        return hit.collider != null;
    }

    // Desenha o CircleCast no Gizmos para visualiza��o no editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 circleCastPosition = (Vector2)transform.position + new Vector2(circleCastOffset.x * direction, circleCastOffset.y);
        Gizmos.DrawWireSphere(circleCastPosition, circleCastRadius);
    }

    // Habilita a persegui��o e o CapsuleCollider
    public void PerseguirFilho()
    {
        Debug.Log("Perseguicao");
        followEnabled = true;
        capsuleCollider.enabled = true;
    }

    // Detec��o de colis�es
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo") && !isGrounded)
        {
            rb.AddForce(Vector2.up * 8f, ForceMode2D.Impulse);
        }else
        {
            jumpEnabled = false;
        }
    }
}
