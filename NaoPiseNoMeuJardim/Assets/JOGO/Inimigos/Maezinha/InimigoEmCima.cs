using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InimigoEmCima : MonoBehaviour
{
    public GameObject Projetil;
    public float tempoSpawn;
    private JARDIM jardim;
    private ScriptPersonagem player;
    
    void Start(){
        jardim = FindObjectOfType<JARDIM>();
        player = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        if(jardim.IniciarJogo == true && player.triggouComTagPararCorrida == true){
            ArremensarProjetil();
        }
    }

    public void ArremensarProjetil()
    {
        tempoSpawn += Time.deltaTime;
        if(tempoSpawn > 2f)
        {
            Vector3 projetil = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject ProjetilLancado = Instantiate(Projetil, projetil, Quaternion.identity);            
            tempoSpawn = 0f;
            Destroy(ProjetilLancado, 1.5f);
        }
    }
}
