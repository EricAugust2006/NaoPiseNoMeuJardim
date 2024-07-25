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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Interact();
        Pular();
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

        // Atualiza animação e flip
        animator.SetFloat("Velocidade", Mathf.Abs(VelX));

        if (VelX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (VelX < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (VelX != 0)
        {
            animator.SetInteger("Transition", 1);
        }
        else
        {
            animator.SetInteger("Transition", 0);
        }
    }

    private void DetectarChao()
    {
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, 0.2f, oQueEhChao);
        animator.SetBool("chao", taNoChao);

        if (taNoChao)
        {
            pulando = false;
        }
    }

    private void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && taNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            pulando = true;
            animator.SetBool("Pulando", true);
        }

        if (!taNoChao && pulando)
        {
            animator.SetBool("Pulando", true);
        }
        else if (taNoChao && !pulando)
        {
            animator.SetBool("Pulando", false);
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
