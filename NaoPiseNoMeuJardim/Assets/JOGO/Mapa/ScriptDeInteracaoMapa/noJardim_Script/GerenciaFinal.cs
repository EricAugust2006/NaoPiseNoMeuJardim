using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciaFinal : MonoBehaviour
{

    public GameObject cutsScene;
    private Temporizador temporizador;
    private ScriptPersonagem player;
    public bool cutsCeneAtivada = false;
    void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        cutsScene.SetActive(false);
        temporizador = FindObjectOfType<Temporizador>();
    }

    void Update()
    {
        podeEscaparDoCastigo();

        if (cutsCeneAtivada == true)
        {
            Time.timeScale = 0f;
        }
        return;

    }

    public void podeEscaparDoCastigo()
    {
        if (temporizador.tempoAtual >= temporizador.tempoMaximo && player.triggouComTagPararCorrida == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                cutsCeneAtivada = true;
                cutsScene.SetActive(true);
            }
        }
    }

    IEnumerator cutsCene(){
        yield return null;
    }
}
