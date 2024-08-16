using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    [Header("Movimentacao")]
    private Rigidbody2D rb;
    [SerializeField] public float speed = 5f;

    [Header("Pulo")]
    public bool pulando = false;
    public float forcaPulo = 7f;

    [Header("Detecta Chao")]
    public Transform detectaChao;
    public LayerMask oQueEhChao;
    private bool wasGrounded;
    public bool taNoChao;

    [Header("Plataforma")]
    private Collider2D col;
    public Transform detectaPlataforma;
    public LayerMask oQueEhPlataforma;
    private bool wasPlataformed;
    public bool taNaPlataforma;

    [Header("IInteractable")]
    public IInteractable interactable;

    [Header("Interacao")]
    public GameObject botaoInteracao;

    [Header("Animacao e Flip")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool podePular = true;

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
        if (podePular)
        {
            Jump();
        }
        AtualizarAnimacoes();
        CuidarLayer();
        MudarPlataforma();
    }

    private void FixedUpdate()
    {
        Movimentar();
        DetectarChao();
    }

    public void MudarPlataforma()
    {
        if (Input.GetButtonDown("Jump") && taNoChao && !taNaPlataforma)
        {
            StartCoroutine(mudarPlataforma());
        }
        if (Input.GetButtonDown("VerticalDown") && !taNoChao && taNaPlataforma)
        {
            StartCoroutine(mudarPlataforma());
        }
    }

    bool EstaNaPlataforma()
    {
        return col.IsTouchingLayers(oQueEhPlataforma);
    }

    IEnumerator mudarPlataforma()
    {
        podePular = false;

        gameObject.layer = LayerMask.NameToLayer("Delimitador");

        yield return new WaitForSeconds(0.8f);

        gameObject.layer = LayerMask.NameToLayer("Default");

        podePular = true;
    }

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
        taNaPlataforma = Physics2D.OverlapCircle(detectaPlataforma.position, 0.4f, oQueEhPlataforma);

        if (taNoChao || taNaPlataforma)
        {
            animator.SetBool("Caindo", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = taNoChao ? Color.green : Color.red;
        Gizmos.DrawSphere(detectaChao.position, 0.4f);

        Gizmos.color = taNaPlataforma ? Color.blue : Color.red;
        Gizmos.DrawSphere(detectaPlataforma.position, 0.4f);
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (podePular && taNoChao || taNaPlataforma)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                animator.SetTrigger("Jump");
            }
        }
    }

    public void CuidarLayer()
    {
        animator.SetLayerWeight(1, taNoChao ? 0 : 1);
        animator.SetLayerWeight(1, taNaPlataforma ? 0 : 1);
    }

    public void Empurrar()
    {
        rb.velocity = new Vector2(rb.velocity.x, 3);
        animator.SetBool("Caindo", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ObjetoImpulso")
        {
            StartCoroutine(mudarPlataforma());
            StartCoroutine(timeBackImpulse());
        }

        if(collision.gameObject.tag == "PrenderPersonagem")
        {
            speed = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    IEnumerator timeBackImpulse()
    {
        rb.velocity = new Vector2(rb.velocity.x, 12);
        yield return null;
        animator.SetTrigger("Jump");
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.Interact();
        }
    }

    private void AtualizarAnimacoes()
    {
        // Se estiver caindo (velocidade vertical negativa)
        if (rb.velocity.y < 0)
        {
            animator.SetBool("Caindo", true);
        }

        // Se estiver no chão ou na plataforma, desativa a animação de caindo
        if (taNoChao || taNaPlataforma)
        {
            animator.SetBool("Caindo", false);
        }

        // Atualiza o estado de `wasGrounded` e `wasPlataformed`
        wasGrounded = taNoChao;
        wasPlataformed = taNaPlataforma;
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
