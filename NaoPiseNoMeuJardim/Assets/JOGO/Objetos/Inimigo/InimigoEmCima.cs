using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InimigoEmCima : MonoBehaviour
{
    public GameObject Projetil;
    public float tempoSpawn;

    void Update()
    {
        ArremensarProjetil();
    }

    public void ArremensarProjetil()
    {
        tempoSpawn += Time.deltaTime;
        if(tempoSpawn > 2f)
        {
            Vector3 projetil = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject ProjetilLancado = Instantiate(Projetil, projetil, Quaternion.identity);            
            tempoSpawn = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            Destroy(Projetil);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            Destroy(Projetil);
        }
    }
}
