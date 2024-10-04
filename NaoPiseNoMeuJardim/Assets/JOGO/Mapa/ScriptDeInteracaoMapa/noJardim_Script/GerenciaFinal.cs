using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciaFinal : MonoBehaviour
{
    public GameObject cutsScene; // O GameObject da cutscene
    private Temporizador temporizador; // Referência ao temporizador
    private ScriptPersonagem player; // Referência ao personagem
    public bool cutsCeneAtivada = false; // Flag para verificar se a cutscene está ativa

    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        cutsScene.SetActive(false); // Desativa a cutscene inicialmente
        temporizador = FindObjectOfType<Temporizador>();
    }

    void Update()
    {
        podeEscaparDoCastigo(); // Verifica se a cutscene pode ser ativada

        // Se a cutscene estiver ativada, pausa o jogo
        if (cutsCeneAtivada)
        {
            Time.timeScale = 0f;
        }
    }

    public void podeEscaparDoCastigo()
    {
        // Verifica se o tempo máximo foi alcançado e se o personagem deve parar
        if (temporizador.tempoAtual >= temporizador.tempoMaximo && player.triggouComTagPararCorrida)
        {
            // Se a tecla F for pressionada, ativa a cutscene
            if (Input.GetKeyDown(KeyCode.F) && !cutsCeneAtivada)
            {
                StartCoroutine(AtivarCutscene());
            }
        }
    }

    private IEnumerator AtivarCutscene()
    {
        cutsCeneAtivada = true; // Marca a cutscene como ativada
        cutsScene.SetActive(true); // Ativa o GameObject da cutscene

        // Aqui você pode adicionar um efeito de transição, como um fade in
        // Exemplo de uma chamada para uma função de fade (precisa ser implementada)
        // yield return StartCoroutine(FadeIn());

        // Espera por um tempo ou até que a cutscene termine
        // Aqui você deve adicionar a lógica para sua cutscene, como animações ou outros elementos
        yield return new WaitForSeconds(5f); // Simula a duração da cutscene

        // Aqui você pode adicionar um efeito de transição, como um fade out
        // yield return StartCoroutine(FadeOut());

        // Finaliza a cutscene
        cutsScene.SetActive(false); // Desativa o GameObject da cutscene
        cutsCeneAtivada = false; // Marca a cutscene como não ativada
        Time.timeScale = 1f; // Retorna o tempo ao normal
    }

    // Exemplos de funções de fade (precisam ser implementadas de acordo com a sua lógica)
    
    private IEnumerator FadeIn()
    {
        // Implementação do fade in
        yield return null;
    }

    private IEnumerator FadeOut()
    {
        // Implementação do fade out
        yield return null;
    }
}
