using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    [Header("Movimentacao")]
    private Rigidbody2D rb;
    [SerializeField] public float speed = 5f;

    [Header("PULO")]
    public bool taNoChao;
    public bool pulando = false;
    public float forcaPulo = 7f;
    public Transform detectaChao;
    public LayerMask oQueEhChao;
    private bool wasGrounded;

    [Header("IInteractable")]
    public IInteractable interactable;

    [Header("INTERACAO")]
    public GameObject botaoInteracao;

    [Header("ANIMACAO E FLIP")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Plataforma")]
    private Collider2D col;
    public LayerMask plataformaLayer;
    public GameObject detectaPlataforma;
    public bool taNaPlataforma;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Interact();
        Jump();
        AtualizarAnimacoes();
        CuidarLayer();
        mudarPlataforma();
    }

    private void FixedUpdate()
    {
        Movimentar();
        DetectarChao();
    }

    public void mudarPlataforma()
    {
        if (Input.GetButtonDown("Jump") && taNoChao && !estaNaPlataforma())
        {
            StartCoroutine(SubirPlataforma());
        }
        if (Input.GetButtonDown("VerticalDown") && !taNoChao && estaNaPlataforma())
        {
            StartCoroutine(DescerPlataforma());
        }
    }

    bool estaNaPlataforma()
    {
        return col.IsTouchingLayers(plataformaLayer);
    }
    
    //COROUTINE PARA DESCER PLATAFORMA
    IEnumerator DescerPlataforma()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.8f);
        col.enabled = true;
    }

    //COROUTINE PARA SUBIR PLATAFORMA
    IEnumerator SubirPlataforma()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.8f);
        col.enabled = true;
    }

    //MOVIMENTAÇÃO DO PERSONAGEM
    public void Movimentar()
    {
        float VelX = Input.GetAxis("Horizontal");
        Vector3 Movement = new Vector3(VelX, 0f, 0f);
        transform.position += Movement * Time.deltaTime * speed;

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
        if (Input.GetButtonDown("Jump") && taNoChao)
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
        animator.SetBool("Caindo", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            if (interactable == collision.GetComponent<IInteractable>())
            {
                interactable = null;
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

    public void DesativarAnimacoes()
    {
        if (animator != null)
        {
            Debug.Log("Desativando animações");
            animator.SetBool("Correndo", false);
            animator.SetBool("Caindo", false);
            animator.SetFloat("Velocidade", 0f);
        }
    }

    public void RestaurarAnimacoes()
    {
        if (animator != null)
        {
            Debug.Log("Restaurando animações");
            animator.SetBool("Correndo", true); 
        }
    }
}