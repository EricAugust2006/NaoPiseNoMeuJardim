using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class animatorScript : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb;
    public GameObject UIapertar;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            anim.SetTrigger("taClicando");
        }        
    }
    
}
