using UnityEngine;
using UnityEngine.UI;

public class JokenpoManager : MonoBehaviour
{
    public GameObject jokenpo;
    public Image jogadorImagemEscolha;
    public Image computadorImagemEscolha;
    public Sprite pedraSprite;
    public Sprite papelSprite;
    public Sprite tesouraSprite;
    public Text textoResultado;

    private string[] chances = { "Pedra", "Papel", "Tesoura" };

    public void jogadorFazerEscolha(string jogadorEscolha)
    {
        // Definir a imagem do jogador
        switch (jogadorEscolha)
        {
            case "Pedra":
                jogadorImagemEscolha.sprite = pedraSprite;
                break;

            case "Papel":
                jogadorImagemEscolha.sprite = papelSprite;
                break;

            case "Tesoura":
                jogadorImagemEscolha.sprite = tesouraSprite;
                break;
        }

        // escolha do compiuter
        string computadorChance = chances[Random.Range(0, chances.Length)];

        // Definir a imagem do compiuter
        switch (computadorChance)
        {
            case "Pedra":
                computadorImagemEscolha.sprite = pedraSprite;
                break;
            case "Papel":
                computadorImagemEscolha.sprite = papelSprite;
                break;
            case "Tesoura":
                computadorImagemEscolha.sprite = tesouraSprite;
                break;
        }

        string result = determinaVencedor(jogadorEscolha, computadorChance);
        textoResultado.text = result;
    }

    private string determinaVencedor(string jogadorEscolha, string computadorChance)
    {
        if (jogadorEscolha == computadorChance)
        {
            return "Empate!";
        }
        else if ((jogadorEscolha == "Pedra" && computadorChance == "Tesoura") ||
                 (jogadorEscolha == "Papel" && computadorChance == "Pedra") ||
                 (jogadorEscolha == "Tesoura" && computadorChance == "Papel"))
        {
            return "Você Ganhou!";
        }
        else
        {
            return "Você Perdeu!";

            jokenpo.SetActive(false);
        }
    }
}
