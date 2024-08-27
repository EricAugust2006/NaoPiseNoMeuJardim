using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.Remoting.Messaging;

public class ScriptMae : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed;
    public float nextWaypointDistance = 3f;
    public float jumpForce = 10f; // For�a do pulo
    public LayerMask obstacleLayer;

    [Header("Custom Behavior")]
    public bool followEnabled = false;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWayPoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Collider2D obstacleDetector;

    [Header("Animator")]
    private Animator animator;

    [Header("GameObjects")]
    // public GameObject gameOver;
    public GameObject Pedra_Papel_Tesoura;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        obstacleDetector = GetComponentInChildren<Collider2D>(); // Assumindo que o Collider2D est� como filho
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
            animator.SetBool("taCorrendo", true);
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
        if (path == null || currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, rb.velocity.y); // Mant�m a velocidade vertical separada

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (obstacleLayer == (obstacleLayer | (1 << other.gameObject.layer)))
        {
            // Pular quando detectar um obst�culo
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void PerseguirFilho()
    {
        Debug.Log("Perseguicao");
        followEnabled = true;
    }

    void OnDrawGizmos()
    {
        // Codigo de Gizmos removido para simplifica��o
    }

    private void OnCollisionEnter2D(Collision2D colisao) 
    {
        if(colisao.gameObject.tag == "Player"){
            Pedra_Papel_Tesoura.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
