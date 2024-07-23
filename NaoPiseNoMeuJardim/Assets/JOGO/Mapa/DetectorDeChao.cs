using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDeChao : MonoBehaviour
{

    ScriptPersonagem player;

    private void Start()
    {
      player = gameObject.transform.parent.gameObject.GetComponent<ScriptPersonagem>();      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "chao" || collision.gameObject.layer == 8)
        {
            player.isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao" || collision.gameObject.layer == 8)
        {
            player.isJumping = true;
        } 
    }


}
