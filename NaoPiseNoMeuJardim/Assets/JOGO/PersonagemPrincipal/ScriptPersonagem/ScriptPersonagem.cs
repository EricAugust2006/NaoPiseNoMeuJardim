using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.UIElements;

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
    public GameObject fimDeJogo;
    public GameObject UIapertar;
    public SistemaDeVida sistemaDeVida;

    [Header("Animacao e Flip")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Apertar para sair")]
    public int quantidadeApertada = 0;

    [Header("Spawn")]
    public GameObject spawnPointSceneA;
    public GameObject spawnPointSceneB;

    [Header("Chances")]
    public int vida = 5;

    [Header("Scripts")]
    private JokenpoManager jokenpoManager;
    private ScriptGanhou dialogoGanhar;
    private InimigosKnockBack chinelo;


    [Header("Cinemachine")]
    private CinemachineFramingTransposer framingTransposer;
    public CinemachineVirtualCamera cinemachine;

    public float targetOrthoSize = 10f;
    public float initialOrthoSize = 6f;
    public float zoomSpeed = 2f;

    private Vector3 originalOffset;
    private Vector3 targetOffset;
    private bool isInTriggerZone = false;
    public float transitionSpeed = 10f;




    [Header("KnockBack Personagem")]
    public float kbForce;
    public float kbCount;
    public float kBTime;

    public bool isKnockRight;
    public bool tomouDano = false;


    public bool eventoTaPreso = false;
    public TextMeshProUGUI textPro;

    private void Awake()
    {
        
        cinemachine = FindObjectOfType<CinemachineVirtualCamera>(); // Find the Cinemachine camera in the scene        
        jokenpoManager = FindObjectOfType<JokenpoManager>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogoGanhar = FindObjectOfType<ScriptGanhou>();
        chinelo = FindObjectOfType<InimigosKnockBack>();

        //if (cinemachine == null)
        //{
        //    cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        //}

        if (cinemachine != null)
        {
            targetOrthoSize = initialOrthoSize; // Inicializar targetOrthoSize com o valor inicial
            cinemachine.m_Lens.OrthographicSize = initialOrthoSize; // Definir o orthoSize inicial

            // Obtém o Framing Transposer do Cinemachine Virtual Camera
            framingTransposer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                // Define o valor inicial do offset
                originalOffset = new Vector3(framingTransposer.m_TrackedObjectOffset.x, 4.05f, framingTransposer.m_TrackedObjectOffset.z);
                targetOffset = originalOffset;

                // Aplicar o valor inicial ao Framing Transposer
                framingTransposer.m_TrackedObjectOffset = originalOffset;
            }
        }   
        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrada!");
        }
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

        AjustarZoomCamera();
        AjustarOffSetCamera();
        Agachar();
    }

    private void FixedUpdate()
    {
        //Movimentar();
        KnockLogic(); //substituiu o Movimentar();
        DetectarChao();
   
    }

    void KnockLogic()
    {
        if (kbCount < 0 && tomouDano == false)
        {
            Movimentar();
        }
        else if (tomouDano == true)
        {
            animator.SetTrigger("dano");

            // Verificação da direção do knockback
            if (isKnockRight == true) // Knockback para a direita
            {
                animator.SetTrigger("dano");

                Debug.Log("Aplicando knockback para a direita");
                rb.velocity = new Vector2(kbForce, kbForce);
            }
            else // Knockback para a esquerda
            {
                animator.SetTrigger("dano");

                Debug.Log("Aplicando knockback para a esquerda");
                rb.velocity = new Vector2(-kbForce, kbForce);
            }

            tomouDano = false; // Resetando o estado de dano
        }

        kbCount -= Time.deltaTime; // Diminuindo o tempo de knockback
    }

    public void Invulnerabilidade(){

    }

    IEnumerator tempoInvulneravel(){

        yield return null;
    }

    public void Agachar() {
        
    if (Input.GetKeyDown(KeyCode.LeftShift)) {
        bool taAgachado = animator.GetBool("taAgachado");
        animator.SetBool("taAgachado", !taAgachado);

        if (animator.GetBool("taAgachado")) {
            speed = 4.5f;
        } else {
            speed = 8f;
            animator.SetTrigger("sairDoAgachar");
        }
    }

    bool estaSeMovendo = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

    if (animator.GetBool("taAgachado") && estaSeMovendo) {
        animator.SetBool("andarAgachado", true);
    } 
    else {
        animator.SetBool("andarAgachado", false);
    }
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
            animator.ResetTrigger("Jump");
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
                    // animator.SetBool("Caindo", false);
                }
            }
        }
    }

    // =================================================================================
    // ================================== TA PRESO =====================================
    // =================================================================================

    public void EstaLivre()
    {
        if(taPreso == true){
            if (Input.GetKeyUp(KeyCode.E))
            {
                quantidadeApertada++;
                textPro.text = quantidadeApertada.ToString();
            }

            if (quantidadeApertada >= 5)
            {
                UIapertar.SetActive(false);
                taPreso = false;
                animator.SetBool("taPreso", false);
                quantidadeApertada = 0;
            }
        }
    }

    // =================================================================================
    // ================================ PLATAFORMAS ====================================
    // =================================================================================

    public void TomouDanoDeCima()
    {
        if (taNaPlataforma)
        {
            TomouDanoNaPlataforma();
        }
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

    IEnumerator mudarPlataforma()
    {
        podePular = false;
        gameObject.layer = LayerMask.NameToLayer("Delimitador");
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
        podePular = true;
    }

    IEnumerator timeBackImpulse()
    {
        rb.velocity = new Vector2(rb.velocity.x, 12);
        yield return null;
        animator.SetTrigger("Jump");
    }

    // =================================================================================
    // ================================== IMPULSO ======================================
    // =================================================================================
    public void Empurrar()
    {
        rb.velocity = new Vector2(rb.velocity.x, 3);
        animator.SetBool("Caindo", false);
    }

    // =================================================================================
    // ======================== PARTE DE ANIMAÇÃO ANIMAÇÃO =============================
    // =================================================================================

    private void AtualizarAnimacoes()
    {
        if (rb.velocity.y < 0)
        {
            animator.SetBool("Caindo", true);
        }
        if (taNoChao || taNaPlataforma)
        {
            animator.SetBool("Caindo", false);
            // animator.ResetTrigger("Jump");
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

    public void CuidarLayer()
    {
        animator.SetLayerWeight(1, taNoChao ? 0 : 1);
        animator.SetLayerWeight(1, taNaPlataforma ? 0 : 1);
    }
    // =================================================================================
    // ============================== CINEMACHINE ZOOM =================================
    // =================================================================================

    public void VoaPassarin(){

        if(framingTransposer != null)
        {
            targetOffset = new Vector3(originalOffset.x, 0, originalOffset.z);
            isInTriggerZone = true;
        }

        if (cinemachine != null) //se cinemachine for diferente de null(nada), ou seja,  se tiver uma cinemachine vai retornar meu targetrthoSize para 10.
        {
            targetOrthoSize = 10f;
        }

        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrada!");
        }
    }

    private void AjustarOffSetCamera()
    {
        if(framingTransposer != null)
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                targetOffset,
                Time.deltaTime * transitionSpeed
                );
        }
    }


    private void AjustarZoomCamera()
    {
        if (cinemachine != null)
        {
            cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(
                cinemachine.m_Lens.OrthographicSize,
                targetOrthoSize,
                zoomSpeed * Time.deltaTime
                );
        }
    }

    // =================================================================================
    // ============================= PARTE DAS COLISÕES ================================
    // =================================================================================

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
            UIapertar.SetActive(true);
            quantidadeApertada = 0;
            taPreso = true;
            animator.SetBool("taPreso", true);
        }

        if(collision.gameObject.tag == "Municao")
        {
            Debug.Log("A folha me encostou");
            StartCoroutine(TomouDanoNaPlataforma());
        }
        
        if(collision.gameObject.tag == "DarZoom"){
            VoaPassarin();
            isInTriggerZone = true;
        }
        
        if(collision.gameObject.tag == "Ganhar"){
            dialogoGanhar.StartDialogue();
        }

        if (collision.gameObject.tag == "chinelo")
        {
           tomouDano = true;
           sistemaDeVida.vida--;
           Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "plataformaInimigo")
        {
            tomouDano = true;
            animator.SetTrigger("dano");
            sistemaDeVida.vida--;
        }
    }

        private void OnTriggerExit2D(Collider2D collision)
    {    
        animator.SetBool("taPreso", false);

        if (collision.gameObject.CompareTag("DarZoom"))
        {
            targetOffset = originalOffset;
            isInTriggerZone = false;
            targetOrthoSize = initialOrthoSize; // Retornar ao tamanho inicial quando sair do trigger
        }
    }

    //public void OnCollisionEnter2D(Collision2D collisionChinelo)
    //{
    //    if (collisionChinelo.gameObject.tag == "chinelo")
    //    {
    //        Destroy(collisionChinelo.gameObject);
    //    }
    //}
}
