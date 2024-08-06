using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDoJogo : MonoBehaviour
{
    private ScriptPersonagem personagemScript;

    [Header("GameObjects")]
    [SerializeField] GameObject SairDoEventoTelefone;


    private void Start()
    {
        personagemScript = FindObjectOfType<ScriptPersonagem>();
    }

    private void Update()
    {
        
    }
    
    public void SairEvento()
    {
        personagemScript.enabled = true;
        SairDoEventoTelefone.SetActive(false);
        personagemScript.RestaurarAnimacoes();
    }
}
