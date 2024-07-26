using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personage : MonoBehaviour
{
    [Header("Movimentacao")]
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    [Header("PULO")]
    public bool taNoChao;
    public float forcaPulo = 7f;
    public Transform detectaChao;
    public LayerMask oQueEhChao;

    [Header("ANIMACAO E FLIP")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool wasGrounded; // Flag para verificar se o personagem estava no chão no frame anterior

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        pular();
        AtualizarAnimacoes();
        CuidarLayer();
    }

    private void FixedUpdate()
    {
        Movimentar();
        DetectarChao();
    }

    private void Movimentar()
    {
        float VelX = Input.GetAxis("Horizontal");
        Vector3 Movement = new Vector3(VelX, 0f, 0f);
        transform.position += Movement * Time.deltaTime * speed;

        // atualiza animacao e flip
        animator.SetFloat("Velocidade", Mathf.Abs(VelX));

        if (VelX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (VelX < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetBool("Correndo", VelX != 0);
    }

    private void DetectarChao()
    {
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, 0.2f, oQueEhChao);
        if (taNoChao)
        {
            animator.SetBool("Caindo", false);
        }
    }

    private void pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && taNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            animator.SetTrigger("pular");
        }
    }

    private void AtualizarAnimacoes()
    {
        if (rb.velocity.y < 0 && !taNoChao && wasGrounded)
        {
            animator.SetBool("Caindo", true);
            wasGrounded = false;
        }

        if (taNoChao && !wasGrounded)
        {
            animator.SetBool("Caindo", false);
            wasGrounded = true;
        }
    }

    public void CuidarLayer()
    {
        if (!taNoChao)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }
}
