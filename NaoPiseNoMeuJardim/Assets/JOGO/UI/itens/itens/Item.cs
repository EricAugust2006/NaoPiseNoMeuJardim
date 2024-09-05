using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Inventário/Item")]
public class Item : ScriptableObject
{
    public string nomeItem;
    public Sprite iconeItem;
    public bool empilhavel;
    public int quantidadeMaxima;

}
