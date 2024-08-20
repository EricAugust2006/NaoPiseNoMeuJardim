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
    public bool podePular = true;
    public bool taPreso = false;

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

    [Header("GameObjects")]
    public GameObject botaoInteracao;
    public GameObject prefabMunicao;

    [Header("Animacao e Flip")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Apertar para sair")]
    public int quantidadeApertada = 0;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (podePular)
        {
            Jump();
        }
        AtualizarAnimacoes();
        CuidarLayer();
        MudarPlataforma();
        EstaLivre();
    }

    private void FixedUpdate()
    {
        Movimentar();
        DetectarChao();
    }

    public void MudarPlataforma()
    {
        if (taPreso == false){
            if (Input.GetButtonDown("Jump") && taNoChao && !taNaPlataforma)
            {
                StartCoroutine(mudarPlataforma());
            }
            if (Input.GetButtonDown("VerticalDown") && !taNoChao && taNaPlataforma)
            {
                StartCoroutine(mudarPlataforma());  
            }
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
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
        podePular = true;
    }

    IEnumerator TomouDanoNaPlataforma()
    {
        Empurrar();
        podePular = false;
        col.enabled = false;
        //gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(1f);
        //gameObject.layer = LayerMask.NameToLayer("Default");
        col.enabled = true;
        podePular = true;
    }

    public void Movimentar()
    {
        if (taPreso == false)
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
        if(taPreso == false)
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
    }

    public void EstaLivre()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            quantidadeApertada++;
        }

        if (quantidadeApertada == 5)
        {
            taPreso = false;
            animator.SetBool("taPreso", false);
            quantidadeApertada = 0;
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

    public void TomouDanoDeCima()
    {
        if (taNaPlataforma)
        {
            TomouDanoNaPlataforma();
        }
    }

    IEnumerator timeBackImpulse()
    {
        rb.velocity = new Vector2(rb.velocity.x, 12);
        yield return null;
        animator.SetTrigger("Jump");
    }
    private void AtualizarAnimacoes()
    {
        if (rb.velocity.y < 0)
        {
            animator.SetBool("Caindo", true);
        }
        if (taNoChao || taNaPlataforma)
        {
            animator.SetBool("Caindo", false);
        }
        wasGrounded = taNoChao;
        wasPlataformed = taNaPlataforma;
    }

    public void DesativarAnimacoes()
    {
        if (animator != null)
        {
            Debug.Log("Desativando anima��es");
            animator.SetBool("Correndo", false);
            animator.SetBool("Caindo", false);
            animator.SetFloat("Velocidade", 0f);
        }
    }

    public void RestaurarAnimacoes()
    {
        if (animator != null)
        {
            Debug.Log("Restaurando anima��es");
            animator.SetBool("Correndo", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ObjetoImpulso")
        {
            StartCoroutine(mudarPlataforma());
            StartCoroutine(timeBackImpulse());
            animator.SetBool("Caindo", false);
        }

        if (collision.gameObject.tag == "PrenderPersonagem")
        {
            quantidadeApertada = 0;
            taPreso = true;
            animator.SetBool("taPreso", true);
        }

        if(collision.gameObject.tag == "Municao")
        {
            Debug.Log("A folha me encostou");
            StartCoroutine(TomouDanoNaPlataforma());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("taPreso", false);
    }
}
