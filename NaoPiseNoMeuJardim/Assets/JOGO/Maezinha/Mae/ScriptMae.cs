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
    public float speed;
    public float nextWaypointDistance = 3f;
    public float jumpForce = 10f; // Força do pulo
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
    public GameObject Pedra_Papel_Tesoura;
    public GameObject coliderFicarNoChao;
    public GameObject resultado;

    [Header("Scripts")]
    private JARDIM jardim;

    [Header("Booleanos")]
    public bool jokenpoEventoAtivado = false;
    private bool eventoDesativadoTemporariamente = false;

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        coliderFicarNoChao.SetActive(false);
        jardim = FindObjectOfType<JARDIM>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        obstacleDetector = GetComponentInChildren<Collider2D>(); // Assumindo que o Collider2D está como filho
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled && jardim.IniciarJogo == true)
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

        rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, rb.velocity.y); // Mantém a velocidade vertical separada

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
            // Pular quando detectar um obstáculo
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void PerseguirFilho()
    {
        Debug.Log("Perseguicao");
        followEnabled = true;
    }

    private void OnCollisionEnter2D(Collision2D colisao) 
    {
        if (colisao.gameObject.tag == "Player" && jardim.IniciarJogo == true && !eventoDesativadoTemporariamente)
        {
            resultado.SetActive(false);
            jokenpoEventoAtivado = true;
            GetComponent<Collider2D>().enabled = true;
            Pedra_Papel_Tesoura.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void DesativarEventoTemporariamente(float duracao)
    {
        StartCoroutine(DesativarEventoCoroutine(duracao));
    }

    private IEnumerator DesativarEventoCoroutine(float duracao)
    {
        eventoDesativadoTemporariamente = true;
        yield return new WaitForSecondsRealtime(duracao);
        eventoDesativadoTemporariamente = false;
    }
}
