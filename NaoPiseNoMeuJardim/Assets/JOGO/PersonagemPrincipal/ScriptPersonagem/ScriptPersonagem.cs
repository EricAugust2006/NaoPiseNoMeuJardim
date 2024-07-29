using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    [Header("Movimentacao")]
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    [Header("PULO")]
    public bool taNoChao;
    public bool pulando = false;
    public float forcaPulo = 7f;
    public Transform detectaChao;
    public LayerMask oQueEhChao;

    //Contrato de eventos
    public IInteractable interactable;

    [Header("INTERACAO")]
    public GameObject botaoInteracao;

    [Header("ANIMACAO E FLIP")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool wasGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Interact();
        Jump();
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
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, 0.4f, oQueEhChao);
        if (taNoChao)
        {
            animator.SetBool("Caindo", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && taNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            animator.SetTrigger("Jump");
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

    public void Empurrar()
    {
        rb.velocity = new Vector2(rb.velocity.x, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            interactable = collision.GetComponent<IInteractable>();
            //botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            if (interactable == collision.GetComponent<IInteractable>())
            {
                interactable = null;
                //botaoInteracao.SetActive(false);
            }
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.Interact();
        }
    }
}