using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciaFinal : MonoBehaviour
{
    private ScriptPersonagem player;
    public float speedautomatico = 8f;
    public List<GameObject> paredes;
    public GameObject paredeTriggerInicia;
    private void Start()
    {
        player = FindObjectOfType<ScriptPersonagem>();
        
        if (paredes == null)
        {
            paredes = new List<GameObject>();
        }
    }
    public void desativarParedes()
    {
        foreach (GameObject parede in paredes)
        {
            if (parede != null)
            {
                parede.SetActive(false);
            }
        }
    }

    public void ativarParedes()
    {
        foreach (GameObject parede in paredes)
        {
            if (parede != null)
            {
                parede.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.MoverAutomaticamente();
            desativarParedes();
        }
    }

    private void MoverAutomaticamente()
    {
        transform.Translate(Vector2.right * speedautomatico * Time.deltaTime);
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pararCorrida"))
        {
            player.movendoAutomaticamente = true;
            paredeTriggerInicia.SetActive(false);
        }
    }
}
