using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScriptMae : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 1.5f; // Ajustado para considerar dois blocos
    public float jumpModifier = 1.5f; // Ajustado para garantir a altura necessária
    public float jumpCheckOffset = 0.1f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float circleCastRadius = 1f; // Raio do CircleCast
    public Vector2 circleCastOffset = new Vector2(1f, 0f); // Offset para o CircleCast

    [Header("Custom Behavior")]
    public bool followEnabled = false;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWayPoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;
    Collider2D triggerCollider;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        capsuleCollider.enabled = false;
        triggerCollider = GetComponentInChildren<Collider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, 2f, groundLayer);

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        // Define a velocidade diretamente para manter constante
        rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, rb.velocity.y);

        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement || IsObstacleInFront())
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

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

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private bool IsObstacleInFront()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 circleCastPosition = (Vector2)transform.position + new Vector2(circleCastOffset.x * direction, circleCastOffset.y);
        RaycastHit2D hit = Physics2D.CircleCast(circleCastPosition, circleCastRadius, Vector2.right * direction, nextWaypointDistance, obstacleLayer);
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 circleCastPosition = (Vector2)transform.position + new Vector2(circleCastOffset.x * direction, circleCastOffset.y);
        Gizmos.DrawWireSphere(circleCastPosition, circleCastRadius);
    }

    public void PerseguirFilho()
    {
        Debug.Log("Perseguicao");
        followEnabled = true;
        capsuleCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }

        if (collision.gameObject.CompareTag("ObjetoNaFrente"))
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }

        if (collision.gameObject.CompareTag("Algo"))
        {
            // Lógica adicional aqui, se necessário
        }
    }
}
