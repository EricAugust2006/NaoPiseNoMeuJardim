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
    [SerializeField] public float speed;

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
    public GameObject desativarColisaoPlataforma;
    public GameObject botaoInteracao;
    public GameObject prefabMunicao;
    public GameObject fimDeJogo;
    public GameObject UIapertar;
    public SistemaDeVida sistemaDeVida;
    public GameObject paredeInvisivel;
    public GameObject GameObjectdetectaPlataforma;
    public GameObject temporizadorIniciar;

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
    public float velocidadeCorridaInfinita = 8f;
    public float distanciaScene = 100f;
    public float velocidadeScene = 5f;
    public bool movendoAutomaticamente = false;
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
    public float velocidadeDescidaRapida = 20f;  // Velocidade extra ao descer
    public float raioDetectaChao = 0.2f;  // Raio para a detecção

    public Collider2D colisorGameObjectPlataforma;

    private void Awake()
    {
        // Verifica se a cena atual é "JardimJogo"
        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            // Encontre o Canvas primeiro
            GameObject canvas = GameObject.Find("UI/HUD");
            if (canvas != null)
            {
                // Em seguida, encontre o temporizador dentro do Canvas
                temporizadorIniciar = canvas.transform.Find("Temporizador")?.gameObject;

                if (temporizadorIniciar != null)
                {
                    DontDestroyOnLoad(temporizadorIniciar);
                }
                else
                {
                    Debug.LogError("Temporizador não encontrado dentro do Canvas HUD!");
                }
            }
            else
            {
                Debug.LogError("Canvas UI/HUD não encontrado!");
            }
        }
        else
        {
            // Garante que a variável seja nula fora da cena "JardimJogo"
            temporizadorIniciar = null;
        }

        // verificaVariaveisEntreCena();

        // temporizadorIniciar.SetActive(false);
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

        // destroiToupeira.SetActive(false);

        if (cinemachine != null)
        {
            targetOrthoSize = initialOrthoSize; // Inicializar targetOrthoSize com o valor inicial
            cinemachine.m_Lens.OrthographicSize = initialOrthoSize; // Definir o orthoSize inicial

            // Obtém o Framing Transposer do Cinemachine Virtual Camera
            framingTransposer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                // Define o valor inicial do offset no eixo X (originalOffsetX)
                originalOffsetX = new Vector3(framingTransposer.m_TrackedObjectOffset.x, framingTransposer.m_TrackedObjectOffset.y, framingTransposer.m_TrackedObjectOffset.z);
                targetOffsetX = originalOffsetX;

                // Aplicar o valor inicial ao Framing Transposer, mas apenas no eixo X
                framingTransposer.m_TrackedObjectOffset = new Vector3(originalOffsetX.x, framingTransposer.m_TrackedObjectOffset.y, framingTransposer.m_TrackedObjectOffset.z);
            }
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrada!");
        }
    }
    public void verificaVariaveisEntreCena()
    {
        if (SceneManager.GetActiveScene().name == "JardimJogo" && temporizadorIniciar != null)
        {
            temporizadorIniciar.SetActive(true);
        }
        else if (temporizadorIniciar != null)
        {
            temporizadorIniciar.SetActive(false);
        }
    }

    private void Update()
    {
        // verificaVariaveisEntreCena();
        descerRapido();

        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            if (pararCorrida)
            {
                MudarCameraParaDireita();
            }

            if (Input.GetKeyDown(KeyCode.E) && jardim.eventoLigado)
            {
                // Remove a parede invisível e inicia a corrida automática
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
            // Permite o movimento manual caso o evento do jardim tenha sido finalizado
            Movimentar();
        }


        if (podePular)
        {
            Jump();
        }

        AtualizarAnimacoes();
        EstaLivre();
    }

    private void FixedUpdate()
    {
        Movimentar();
        DetectarChao();
    }

    // void KnockLogic()
    // {
    //     if(jardim.eventoLigado == false){
    //         if (kbCount < 0 && tomouDano == false)
    //         {
    //             Movimentar();
    //         }
    //         else if (tomouDano == true)
    //         {
    //             animator.SetTrigger("dano");
    //             // Verificação da direção do knockback
    //             if (isKnockRight == true) // Knockback para a direita
    //             {
    //                 animator.SetTrigger("dano");

    //                 Debug.Log("Aplicando knockback para a direita");
    //                 rb.velocity = new Vector2(kbForce, kbForce);
    //             }
    //             else // Knockback para a esquerda
    //             {
    //                 animator.SetTrigger("dano");

    //                 Debug.Log("Aplicando knockback para a esquerda");
    //                 rb.velocity = new Vector2(-kbForce, kbForce);
    //             }
    //             tomouDano = false; // Resetando o estado de dano
    //         }
    //         kbCount -= Time.deltaTime; // Diminuindo o tempo de knockback
    //     } 
    //     else {
    //         return;
    //     }
    // }


    // public void Agachar()
    // {
    //     if (Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         bool taAgachado = animator.GetBool("taAgachado");
    //         animator.SetBool("taAgachado", !taAgachado);

    //         if (animator.GetBool("taAgachado"))
    //         {
    //             //speed = 4.5f;
    //         }
    //         else
    //         {
    //             speed = 8f;
    //             animator.SetTrigger("sairDoAgachar");
    //         }
    //     }

    //     bool estaSeMovendo = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

    //     if (animator.GetBool("taAgachado") && estaSeMovendo)
    //     {
    //         animator.SetBool("andarAgachado", true);
    //     }
    //     else
    //     {
    //         animator.SetBool("andarAgachado", false);
    //     }
    // }

    private void MoverAutomaticamente()
    {
        float speedautomatico = speed * 1.5f;
        //animator.SetBool("Correndo", true);
        transform.Translate(Vector2.right * speedautomatico * Time.deltaTime);
    }

    public void Movimentar()
    {
        //if (taPreso == false)
        //{
        //    float VelX = Input.GetAxis("Horizontal");
        //    Vector3 Movement = new Vector3(VelX, 0f, 0f);
        //    transform.position += Movement * Time.deltaTime * speed;

        //    animator.SetFloat("Velocidade", Mathf.Abs(VelX));

        //    if (VelX > 0)
        //    {
        //        spriteRenderer.flipX = false;
        //    }
        //    else if (VelX < 0)
        //    {
        //        spriteRenderer.flipX = true;
        //    }

        //    animator.SetBool("Correndo", VelX != 0);
        //}

        // Movimento do personagem enquanto não está no estado de corrida infinita
        if (SceneManager.GetActiveScene().name == "JardimJogo")
        {
            // Verifica se o personagem não está em corrida automática e o evento não está ativado
            if (!emCorridaInfinita && !movendoAutomaticamente && !jardim.IniciarJogo)
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
        else
        {
            // Movimentação fora do Jardim
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

    private void MudarCameraParaDireita()
    {
        if (framingTransposer != null)
        {
            // Defina o valor do offset no eixo X para mover a câmera para a direita
            framingTransposer.m_TrackedObjectOffset = new Vector3(originalOffsetX.x + offSetX, framingTransposer.m_TrackedObjectOffset.y, framingTransposer.m_TrackedObjectOffset.z);
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
        if (taPreso == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // destroiToupeira.enabled = false;
                if (podePular && taNoChao || taNaPlataforma)
                {
                    rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                    animator.SetTrigger("Jump");
                }
            }
        }
    }

    public void VerificarQueda()
    {
        Debug.Log("O personagem está caindo!");
        animator.SetTrigger("Caiu"); // Chama o trigger de animação "Caiu"
    }
    public void descerRapido()
    {
        taNoChao = Physics2D.OverlapCircle(detectaChao.position, raioDetectaChao, oQueEhChao);

        if (!taNoChao && Input.GetButtonDown("VerticalDown") && !taNaPlataforma)
        {
            // destroiToupeira.SetActive(true);
            StartCoroutine(trocarLayerPlayer());
            DescerRapidamente();
        }
    }

    public void DescerRapidamente()
    {
        rb.velocity = new Vector2(rb.velocity.x, -velocidadeDescidaRapida);
    }

    IEnumerator trocarLayerPlayer()
    {
        gameObject.tag = "PlayerAtk";
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Player";
    }

    // =================================================================================
    // ================================== TA PRESO =====================================
    // =================================================================================

    public void EstaLivre()
    {
        if (taPreso == true)
        {
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
        Empurrar();
        podePular = false;
        col.enabled = false;
        //gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(.5f);
        //gameObject.layer = LayerMask.NameToLayer("Default");
        col.enabled = true;
        podePular = true;
    }

    // IEnumerator mudarPlataforma()
    // {
    //     col.isTrigger = false; // Restaura a colisão normal
    //     podePular = false;
    //     // gameObject.layer = LayerMask.NameToLayer("Delimitador");
    //     yield return new WaitForSeconds(.5f);
    //     // gameObject.layer = LayerMask.NameToLayer("Default");
    //     podePular = true;
    // }

    IEnumerator timeBackImpulse()
    {
        rb.velocity = new Vector2(rb.velocity.x, 12);
        yield return null;
        animator.SetTrigger("Jump");
    }

    // =================================================================================
    // ================================== IMPULSO ======================================
    // =================================================================================
    public void InimigoEmpurrar()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 15);
        animator.SetBool("Caindo", false);
    }

    public void Empurrar()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 10);
        animator.SetBool("Caindo", false);
    }

    public void EmpurrarEspelho()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 5);
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
            // Aqui ajustamos apenas o eixo X
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                new Vector3(targetOffsetX.x, framingTransposer.m_TrackedObjectOffset.y, framingTransposer.m_TrackedObjectOffset.z),
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

        parallaxCeuUm.movimentoAutomatico = 1f;
        parallaxCeuDois.movimentoAutomatico = 1f;


        parallaxRochasUm.movimentoAutomatico = .8f;
        parallaxRochasDois.movimentoAutomatico = .8f;

        parallaxGramasUm.movimentoAutomatico = .6f;
        parallaxGramasDois.movimentoAutomatico = .6f;

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

    // public void verificaVariaveisEntreCena()
    // {
    //     if (temporizadorIniciar == null)
    //     {
    //         return;
    //     }
    // }

    // =================================================================================
    // ============================= PARTE DAS COLISÕES ================================
    // =================================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pararCorrida")
        {
            triggouComTagPararCorrida = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            spriteRenderer.flipX = false;
            movendoAutomaticamente = false;
            parallaxAtivar = true;
            forcaPulo = 18f;
            // verificaVariaveisEntreCena();
            temporizadorIniciar.SetActive(true);
            VoaPassarin();
            AjustarOffSetCamera();
            MudarCameraParaDireita();
            modificaParallaxAutomatico();
        }

        if (collision.gameObject.tag == "ObjetoImpulso")
        {
            // StartCoroutine(mudarPlataforma());
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
            StartCoroutine(TomouDanoNaPlataforma());
        }

        if (gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "plataformaInimigo")
            {
                animator.SetTrigger("dano");
                Debug.Log("Tomou dano da toupeira");
                StartCoroutine(TomouDanoNaPlataforma());
            }
        }

        if (collision.gameObject.tag == "DarZoom")
        {

        }

        if (collision.gameObject.tag == "Ganhar")
        {
            dialogoGanhar.StartDialogue();
        }

        if (collision.gameObject.tag == "chinelo")
        {
            Empurrar();
            animator.SetTrigger("dano");
            tomouDano = true;
            sistemaDeVida.vida--;
            Destroy(collision.gameObject);
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
}
