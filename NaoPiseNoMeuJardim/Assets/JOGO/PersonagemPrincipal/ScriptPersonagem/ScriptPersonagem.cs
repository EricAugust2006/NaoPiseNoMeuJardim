using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScriptPersonagem : MonoBehaviour
{
    [Header("Movimentacao")]
    public Rigidbody2D rb;

    [SerializeField]
    public float speed;

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
    public Collider2D col;
    public Transform detectaPlataforma;
    public LayerMask oQueEhPlataforma;
    private bool wasPlataformed;
    public bool taNaPlataforma;

    [Header("GameObjects")]
    public GameObject desativarColisaoPlataforma;
    public GameObject botaoInteracao;
    public GameObject prefabMunicao;
    public GameObject fimDeJogo;
    public GameObject UIapertar;
    public SistemaDeVida sistemaDeVida;
    public GameObject paredeInvisivel;
    public GameObject GameObjectdetectaPlataforma;


    [Header("Animacao e Flip")]
    private SpriteRenderer spriteRenderer;
    public Animator animator;

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
    private JARDIM jardim;
    private MoverFundo parallax;
    private MoverFundo moveFundo;

    [Header("Cinemachine")]
    private CinemachineFramingTransposer framingTransposer;
    public CinemachineVirtualCamera cinemachine;
    private bool pararCorrida = false;

    public float targetOrthoSize = 10f;
    public float initialOrthoSize = 6f;
    public float zoomSpeed = 2f;

    public float offSetX;

    private Vector3 originalOffset;
    private Vector3 targetOffset;

    private Vector3 originalOffsetX;
    private Vector3 targetOffsetX;

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

    [Header("Corrida Infinita")]
    public bool emCorridaInfinita = false;
    public bool movendoAutomaticamente = false;
    public float velocidadeCorridaInfinita = 8f;
    public float distanciaScene = 100f;
    public float velocidadeScene = 5f;
    private bool movendoScene = false;
    public Vector2 destination;

    [Header("Bools Parallax")]
    public bool parallaxAtivar = false;
    public bool triggouComTagPararCorrida = false;

    [Header("Corrida Infinita")]
    public MoverFundo parallaxCeuUm;
    public MoverFundo parallaxCeuDois;

    public MoverFundo parallaxRochasUm;
    public MoverFundo parallaxRochasDois;

    public MoverFundo parallaxGramasUm;
    public MoverFundo parallaxGramasDois;

    public MoverFundo parallaxChaoUM;
    public MoverFundo parallaxChaoDOIS;

    public MoverFundo parallaxArvoreUm;
    public MoverFundo parallaxArvoreDois;

    [Header("Descer rápido")]
    public float velocidadeDescidaRapida = 20f;
    public float raioDetectaChao = 0.2f;
    public Collider2D colisorGameObjectPlataforma;
    public float tempoEntreDano = 1f;
    private float ultimoTempoDano = 0f;

    public GameObject colisoresParede1;
    public GameObject colisoresParede2;
    public List<GameObject> colisoresParede;

    public float velocidadeMovimento = 5f;
    private bool movimentoAutomaticoAtivado = false;
    private Vector3 direcaoMovimento = Vector3.right;


    [Header("aAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")]
    public GameObject painelDica;

    private bool painelAtivo = false;
    private bool podeFecharPainel = false;

    private void Awake()
    {
        painelDica.SetActive(false);

        if (colisoresParede == null)
        {
            colisoresParede = new List<GameObject>();
        };
        parallax = FindObjectOfType<MoverFundo>();
        jardim = FindObjectOfType<JARDIM>();
        cinemachine = FindObjectOfType<CinemachineVirtualCamera>(); // Find the Cinemachine camera in the scene
        jokenpoManager = FindObjectOfType<JokenpoManager>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogoGanhar = FindObjectOfType<ScriptGanhou>();
        chinelo = FindObjectOfType<InimigosKnockBack>();
        colisoresParede1.SetActive(false);
        colisoresParede2.SetActive(false);


        if (cinemachine != null)
        {
            targetOrthoSize = initialOrthoSize;
            cinemachine.m_Lens.OrthographicSize = initialOrthoSize;
            targetOrthoSize = initialOrthoSize;
            cinemachine.m_Lens.OrthographicSize = initialOrthoSize;

            framingTransposer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                // Define o valor inicial do offset no eixo X (originalOffsetX)
                // dificil pra aprender, mas é compreensivel
                originalOffsetX = new Vector3(
                    framingTransposer.m_TrackedObjectOffset.x,
                    framingTransposer.m_TrackedObjectOffset.y,
                    framingTransposer.m_TrackedObjectOffset.z
                );
                targetOffsetX = originalOffsetX;

                // Aplicar o valor inicial ao Framing Transposer, mas apenas no eixo X
                // dificil pra aprender, mas é compreensivel
                framingTransposer.m_TrackedObjectOffset = new Vector3(
                    originalOffsetX.x,
                    framingTransposer.m_TrackedObjectOffset.y,
                    framingTransposer.m_TrackedObjectOffset.z
                );
            }
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrada!");
        }
    }
    public void desativarColisores()
    {
        foreach (GameObject paredes in colisoresParede)
        {
            if (paredes != null)
            {
                paredes.SetActive(false);
            }
        }
    }
    public void ativarColisores()
    {
        foreach (GameObject paredes in colisoresParede)
        {
            if (paredes != null)
            {
                paredes.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (podeFecharPainel && Input.GetKeyDown(KeyCode.E))
        {
            FecharPainel();
        }

        if (movendoAutomaticamente)
        {
            rb.linearVelocity = new Vector2(5f, rb.linearVelocity.y);
        }

        AtualizarAnimacoes();
        RestaurarAnimacoes();
        descerRapido();

        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            if (pararCorrida)
            {
                MudarCameraParaDireita();
            }
            if (jardim.eventoLigado)
            {
                paredeInvisivel.SetActive(false);
                animator.SetBool("Correndo", true);
                movendoAutomaticamente = true;
                spriteRenderer.flipX = false;
            }
            AjustarZoomCamera();
            MudarPlataforma();
            CuidarLayer();
        }
        if (movendoAutomaticamente)
        {
            MoverAutomaticamente();
        }
        else
        {
            Movimentar();
        }
        if (podePular)
        {
            Jump();
        }
        AtualizarAnimacoes();
    }

    private void FixedUpdate()
    {
        if (movendoAutomaticamente == true)
        {
            MoverAutomaticamente();
        }
        else
        {
            Movimentar();
        }
        DetectarChao();
    }
    public void MoverAutomaticamente()
    {
        animator.SetBool("Correndo", true);
        animator.SetFloat("Velocidade", 1);
        float speedautomatico = speed;
        transform.Translate(Vector2.right * speedautomatico * Time.deltaTime);
    }
    public void Movimentar()
    {
        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            if (!jardim.IniciarJogo)
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
            else
            {
                float VelX = Input.GetAxis("Horizontal");
                Vector3 Movement = new Vector3(VelX, 0f, 0f);
                transform.position += Movement * Time.deltaTime * speed;
                animator.SetFloat("Velocidade", 1);
            }
        }
        else
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
    void AbrirPainel()
    {
        Time.timeScale = 0f;
        painelDica.SetActive(true);
        painelAtivo = true;
        podeFecharPainel = true;
    }

    void FecharPainel()
    {
        Time.timeScale = 1f;
        painelDica.SetActive(false);
        painelAtivo = false;
        podeFecharPainel = false;
    }

    private void MudarCameraParaDireita()
    {
        if (framingTransposer != null)
        {

            framingTransposer.m_TrackedObjectOffset = new Vector3(
                originalOffsetX.x + offSetX,
                framingTransposer.m_TrackedObjectOffset.y,
                framingTransposer.m_TrackedObjectOffset.z
            );
        }
    }

    private void DetectarChao()
    {
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, 0.4f, oQueEhChao);
        taNaPlataforma = Physics2D.OverlapCircle(
            detectaPlataforma.position,
            0.4f,
            oQueEhPlataforma
        );

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
        if (taPreso == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (podePular && taNoChao || taNaPlataforma)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
                    animator.SetTrigger("Jump");
                }
            }
        }
    }

    public void descerRapido()
    {
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, raioDetectaChao, oQueEhChao);

        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            if (triggouComTagPararCorrida == true)
            {
                if (!taNoChao && Input.GetButtonDown("VerticalDown") && !taNaPlataforma)
                {
                    StartCoroutine(trocarLayerPlayer());
                    DescerRapidamente();
                }
            }
        }
    }

    public void DescerRapidamente()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -velocidadeDescidaRapida);
    }

    IEnumerator trocarLayerPlayer()
    {
        gameObject.tag = "PlayerAtk";
        yield return new WaitForSeconds(.5f);
        gameObject.tag = "Player";
    }
    // ================================ PLATAFORMAS ====================================
    public void TomouDanoDeCima()
    {
        if (taNaPlataforma)
        {
            TomouDanoNaPlataforma();
        }
    }

    public void MudarPlataforma()
    {
        if (taPreso == false)
        {
            if (Input.GetButtonDown("Jump") && taNoChao && !taNaPlataforma)
            {
                colisorGameObjectPlataforma.isTrigger = false;
                GameObjectdetectaPlataforma.SetActive(true);
            }
            if (Input.GetButtonDown("VerticalDown") && !taNoChao && taNaPlataforma)
            {
                GameObjectdetectaPlataforma.SetActive(false);
                colisorGameObjectPlataforma.isTrigger = true;
            }
        }
    }

    bool EstaNaPlataforma()
    {
        return col.IsTouchingLayers(oQueEhPlataforma);
    }

    IEnumerator TomouDanoNaPlataforma()
    {
        podePular = false;
        col.enabled = false;
        yield return new WaitForSeconds(.5f);
        col.enabled = true;
        podePular = true;
    }

    IEnumerator timeBackImpulse()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 12);
        yield return null;
        animator.SetTrigger("Jump");
    }
    // ================================= EMPURRAR ======================================
    public void InimigoEmpurrar()
    {
        // animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        // animator.SetBool("Caindo", false);
    }

    public void Empurrar()
    {
        // animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10);
        // animator.SetBool("Caindo", false);
    }

    public void EmpurrarEspelho()
    {
        // animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5);
        // animator.SetBool("Caindo", false);
    }
    // ======================== PARTE DE ANIMAÇÃO ANIMAÇÃO =============================
    private void AtualizarAnimacoes()
    {
        if (rb.linearVelocity.y < 0)
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

    public void CuidarLayer()
    {
        animator.SetLayerWeight(1, taNoChao ? 0 : 1);
        animator.SetLayerWeight(1, taNaPlataforma ? 0 : 1);
    }
    // ============================== CINEMACHINE ZOOM =================================
    public void VoaPassarin()
    {
        if (framingTransposer != null)
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
        if (framingTransposer != null)
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                new Vector3(
                    targetOffsetX.x,
                    framingTransposer.m_TrackedObjectOffset.y,
                    framingTransposer.m_TrackedObjectOffset.z
                ),
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

    private void modificaParallaxAutomatico()
    {
        parallaxChaoUM.movimentoAutomatico = 8f;
        parallaxChaoDOIS.movimentoAutomatico = 8f;

        parallaxCeuUm.movimentoAutomatico = 2f;
        parallaxCeuDois.movimentoAutomatico = 2f;

        parallaxRochasUm.movimentoAutomatico = 1.5f;
        parallaxRochasDois.movimentoAutomatico = 1.5f;

        parallaxGramasUm.movimentoAutomatico = 1.2f;
        parallaxGramasDois.movimentoAutomatico = 1.2f;

        parallaxArvoreUm.movimentoAutomatico = 8f;
        parallaxArvoreDois.movimentoAutomatico = 8f;

        //parallax effect

        parallaxChaoUM.parallaxEffect = 0f;
        parallaxChaoDOIS.parallaxEffect = 0f;

        parallaxCeuUm.parallaxEffect = 0f;
        parallaxCeuDois.parallaxEffect = 0f;

        parallaxRochasUm.parallaxEffect = 0f;
        parallaxRochasDois.parallaxEffect = 0f;

        parallaxGramasUm.parallaxEffect = 0f;
        parallaxGramasDois.parallaxEffect = 0f;

        parallaxArvoreUm.parallaxEffect = 0f;
        parallaxArvoreDois.parallaxEffect = 0f;
    }

    IEnumerator invencilibdade()
    {
        gameObject.tag = "intangível";

        yield return new WaitForSeconds(1f);

        gameObject.tag = "Player";
    }

    public void IniciarMovimentoAutomatico()
    {
        movendoAutomaticamente = true;
    }
    public void FimDoJogo()
    {
        movimentoAutomaticoAtivado = false;
        Debug.Log("Fim do Jogo!");
    }
    // ============================= PARTE DAS COLISÕES ================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pararCorrida")
        {
            AbrirPainel();
            triggouComTagPararCorrida = true;
            rb.constraints =
            RigidbodyConstraints2D.FreezeRotation;
            spriteRenderer.flipX = false;
            movendoAutomaticamente = false;
            parallaxAtivar = true;
            forcaPulo = 18f;
            VoaPassarin();
            AjustarOffSetCamera();
            MudarCameraParaDireita();
            modificaParallaxAutomatico();
            cinemachine.Follow = null;
            colisoresParede1.SetActive(true);
            colisoresParede2.SetActive(true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "ObjetoImpulso")
        {
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

        if (collision.gameObject.tag == "Municao")
        {
            Destroy(collision.gameObject);
            Debug.Log("A folha me encostou");
            Empurrar();
            StartCoroutine(TomouDanoNaPlataforma());
        }

        if (gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "plataformaInimigo")
            {
                StartCoroutine(invencilibdade());
                Empurrar();
                animator.SetTrigger("dano");
                Debug.Log("Tomou dano da toupeira");
            }
        }

        if (gameObject.tag == "Player" || gameObject.tag == "PlayerAtk")
        {
            if (collision.gameObject.tag == "plantaCarnivora")
            {
                StartCoroutine(invencilibdade());
                Empurrar();
                animator.SetTrigger("dano");
                Debug.Log("Tomou dano da planta carnivora");
            }
        }

        if (collision.gameObject.tag == "Ganhar")
        {
            dialogoGanhar.StartDialogue();
        }

        if (collision.gameObject.tag == "chinelo")
        {
            Empurrar();
            animator.SetTrigger("dano");
            StartCoroutine(invencilibdade());
            tomouDano = true;
            sistemaDeVida.vida--;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("paredes"))
        {
            sistemaDeVida.vida--;
            Empurrar();
            animator.SetTrigger("dano");
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DarZoom"))
        {
            targetOffset = originalOffset;
            isInTriggerZone = false;
            targetOrthoSize = initialOrthoSize;
        }
    }
}