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
    public float chanceSpawn = 0.2f; // Chance de spawn (0.0 a 1.0)

    void Start()
    {
        jardim = FindObjectOfType<JARDIM>();
        player = FindObjectOfType<ScriptPersonagem>();
    }

    void Update()
    {
        if (player.triggouComTagPararCorrida == true)
        {
            ArremensarProjetil();
        }
    }

    public void ArremensarProjetil()
    {
        tempoSpawn += Time.deltaTime;
        if (tempoSpawn > 2f)
        {
            // Verifica se o projétil deve ser spawned com base na chance
            if (Random.value <= chanceSpawn)
            {
                Vector3 posicaoProjetil = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                GameObject ProjetilLancado = Instantiate(Projetil, posicaoProjetil, Quaternion.identity);

                // Adiciona movimento ao projetil se ele tiver um Rigidbody2D
                Rigidbody2D rb = ProjetilLancado.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = new Vector2(-velocidadeProjetil, 0); // Mover na direção x positiva
                }

                // Reinicia o tempo de spawn e destrói o projétil após um tempo
                tempoSpawn = 0f;
                Destroy(ProjetilLancado, tempoSumir);
            }
            else
            {
                // Reinicia o tempo de spawn sem gerar o projétil
                tempoSpawn = 0f;
            }
        }
    }
}
