using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoHorizontal : MonoBehaviour
{
    public GameObject Projetil;
    public float tempoSpawn;
    public float velocidadeProjetil = 5f; // Definir a velocidade do projetil
    private JARDIM jardim;
    private ScriptPersonagem player;
    public float tempoSumir;

    void Start(){
        jardim = FindObjectOfType<JARDIM>();
        player = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        if(player.triggouComTagPararCorrida == true){
            ArremensarProjetil();
        }
    }

    public void ArremensarProjetil()
    {
        tempoSpawn += Time.deltaTime;
        if(tempoSpawn > 2f)
        {
            Vector3 posicaoProjetil = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject ProjetilLancado = Instantiate(Projetil, posicaoProjetil, Quaternion.identity);

            // Adiciona movimento ao projetil se ele tiver um Rigidbody2D
            Rigidbody2D rb = ProjetilLancado.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(-velocidadeProjetil, 0); // Mover na direção x positiva
            }

            tempoSpawn = 0f;
            Destroy(ProjetilLancado, tempoSumir);
        }
    }
}
