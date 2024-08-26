using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Referências aos GameObjects que representam os pontos de spawn
    public GameObject entrouNoQuarto;
    public GameObject saiuDoQuarto;

    public GameObject subiuEscadasPrimeiroAndar;
    public GameObject desceuEscadasPrimeiroAndar;

    public GameObject voltouDoJardim;


    void Start()
    {
        // Recupera o nome da cena anterior
        string previousScene = PlayerPrefs.GetString("previousScene", "");

        // Verifica de qual cena o jogador veio e ajusta a posição de spawn
        if (previousScene == "SegundoAndar" && entrouNoQuarto != null)
        {
            transform.position = entrouNoQuarto.transform.position;
        }
        else if (previousScene == "meuQuarto" && saiuDoQuarto != null)
        {
            transform.position = saiuDoQuarto.transform.position;
        }
        else if(previousScene == "primeiroAndar" && subiuEscadasPrimeiroAndar != null) 
        {
            transform.position = subiuEscadasPrimeiroAndar.transform.position;
        }
        else if(previousScene == "SegundoAndar" && desceuEscadasPrimeiroAndar != null) 
        {
            transform.position = desceuEscadasPrimeiroAndar.transform.position;
        }
        else if(previousScene == "JardimJogo" && voltouDoJardim != null) 
        {
            transform.position = voltouDoJardim.transform.position;
        }
        // else
        // {
        //     // Posição padrão ou outra lógica caso não corresponda a nenhuma cena específica
        //     float x = PlayerPrefs.GetFloat("spawnX", 0);
        //     float y = PlayerPrefs.GetFloat("spawnY", 0);
        //     float z = PlayerPrefs.GetFloat("spawnZ", 0);
        //     transform.position = new Vector3(x, y, z);
        // }
    }
}
