using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPersonagem : MonoBehaviour
{
    //movimentacao
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    //pulo
    public float SpeedJump;
    public float JumpForce;
    public bool isJumping;

    //Contrato de eventos
    private IInteractable interactable;

    //aparecer botão interacao
    public GameObject botaoInteracao;


    //pegando spriterenderer
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
    }

    private void FixedUpdate()
    {
        float VelX = Input.GetAxis("Horizontal");
        Vector3 Movement = new Vector3(VelX, 0f, 0f);
        transform.position += Movement * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
        

        //veio do scriptanimacaopersonagem
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Personagem entrou em contato com um objeto interagível.");
            interactable = collision.GetComponent<IInteractable>();
            botaoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            if (interactable == collision.GetComponent<IInteractable>())
            {
                Debug.Log("Personagem saiu de contato com o objeto interagível.");
                interactable = null;
                botaoInteracao.SetActive(false);
            }
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            Debug.Log("Interagindo com o objeto.");
            interactable.Interact();
        }
    }
}
