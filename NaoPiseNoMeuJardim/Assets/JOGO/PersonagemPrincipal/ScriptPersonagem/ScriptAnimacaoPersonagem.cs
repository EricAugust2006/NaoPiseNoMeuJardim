using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptAnimacaoPersonagem : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ScriptPersonagem movimentoPersonagem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movimentoPersonagem = GetComponent<ScriptPersonagem>();
    }

    private void Update()
    {
        float VelX = Input.GetAxis("Horizontal");
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    animator.SetBool("Pulando", true);
        //    animator.SetBool("Correndo", false);
        //} else {
        //    animator.SetBool("Pulando", false);
        //}
    }
}
