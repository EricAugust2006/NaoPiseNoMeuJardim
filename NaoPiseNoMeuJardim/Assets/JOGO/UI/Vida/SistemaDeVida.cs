using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaDeVida : MonoBehaviour
{
    public int vida;
    public int vidaMaxima;

    public Image[] coracao;
    public Sprite cheio;
    public Sprite vazio;

    public GameObject telaGameOver;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LogicaDeVIda();
    }

    void LogicaDeVIda(){
        if(vida > vidaMaxima){
            vida = vidaMaxima;
        }

        for(int i = 0; i < coracao.Length; i++){
            if(i < vida){
                coracao[i].sprite = cheio;
            }
            else {
                coracao[i].sprite = vazio;
            }

            
            if(i < vidaMaxima){
                coracao[i].enabled = true;
            } else {
                coracao[i].enabled = false;
            }
        }

        if(SceneManager.GetActiveScene().name == "JardimJogo"){
            if(vida == 0){
                Time.timeScale = 0f;
                telaGameOver.SetActive(true);
            }
        }
    }

    // void TelaGameOver(){
        
    // } 
}
