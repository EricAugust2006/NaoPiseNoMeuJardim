using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicaoDeCenas : MonoBehaviour
{
    [Header("GameObject e Slider Carregamento")]
    public GameObject TelaDeCarregamento;
    public Slider TelaDeCarregamentoSlider;

    public void CarregarCena(string sceneName)
    {
        StartCoroutine(TelaDeCarregamentoAsync(sceneName));
    }
    IEnumerator TelaDeCarregamentoAsync(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        TelaDeCarregamento.SetActive(true);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            TelaDeCarregamentoSlider.value = progress;

            yield return null;
        }
    }
}
